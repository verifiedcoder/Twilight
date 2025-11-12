using Autofac;
using Autofac.Core;
using Autofac.Core.Registration;
using CommunityToolkit.Diagnostics;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Reflection;
using Twilight.CQRS.Interfaces;
using Twilight.CQRS.Messaging.Interfaces;

namespace Twilight.CQRS.Messaging.InMemory.Autofac;

/// <summary>
///     <para>
///         Provides a means of dispatching messages. This implementation uses Autofac to resolve a registered message
///         handler from the container and call that handler, passing any appropriate message. This class cannot be
///         inherited.
///     </para>
///     <para>Implements <see cref="IMessageSender" />.</para>
/// </summary>
/// <seealso cref="IMessageSender" />
public sealed class AutofacInMemoryMessageSender(
    ILifetimeScope lifetimeScope,
    ILogger<AutofacInMemoryMessageSender> logger) : IMessageSender
{
    private const string DefaultAssemblyVersion = "1.0.0.0";

    private static string Namespace => typeof(AutofacInMemoryMessageSender).Namespace ?? nameof(AutofacInMemoryMessageSender);

    private static string AssemblyVersion => Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? DefaultAssemblyVersion;

    private static readonly ActivitySource ActivitySource = new(Namespace, AssemblyVersion);

    // Cache for reflection MethodInfo objects to avoid repeated lookups
    private static readonly ConcurrentDictionary<(Type HandlerType, Type MessageType), MethodInfo> MethodCache = new();

    // Cache for closed generic types to avoid repeated MakeGenericType calls
    private static readonly ConcurrentDictionary<(Type GenericType, Type MessageType, Type? ResultType), Type> TypeCache = new();

    /// <inheritdoc />
    public async Task<Result> Send<TCommand>(TCommand command, CancellationToken cancellationToken = default)
        where TCommand : class, ICqrsCommand
    {
        var guardResult = ValidateNotNull(command);
        
        if (guardResult.IsFailed)
        {
            return guardResult;
        }

        using var activity = CreateActivity($"Send {command.GetType()}");

        var assemblyQualifiedName = typeof(ICqrsCommandHandler<TCommand>).AssemblyQualifiedName ?? "Unknown Assembly";

        var handlersResult = TryResolveCommandHandlers<TCommand>(assemblyQualifiedName);
        
        if (handlersResult.IsFailed)
        {
            return Result.Fail(handlersResult.Errors); // Convert Result<T> to Result
        }

        var commandHandlers = handlersResult.Value.ToList();

        var validationResult = ValidateCommandHandlerCount(commandHandlers.Count, assemblyQualifiedName);
        
        if (validationResult.IsFailed)
        {
            return validationResult;
        }

        await commandHandlers[0].Handle(command, cancellationToken);
        
        return Result.Ok();
    }

    /// <inheritdoc />
    public async Task<Result<TResult>> Send<TResult>(ICqrsCommand<TResult> command, CancellationToken cancellationToken = default)
    {
        var guardResult = ValidateNotNull(command);
        
        if (guardResult.IsFailed)
        {
            return guardResult;
        }

        var commandType = command.GetType();
        var genericType = typeof(ICqrsCommandHandler<,>);

        var closedGenericType = TypeCache.GetOrAdd((genericType, commandType, typeof(TResult)), key =>
            key.GenericType.MakeGenericType(key.MessageType, key.ResultType!));

        var assemblyQualifiedName = closedGenericType.AssemblyQualifiedName ?? "Unknown Assembly";

        using var activity = CreateActivity($"Send {commandType}");

        return await ExecuteHandlerWithResult<TResult>(closedGenericType, command, assemblyQualifiedName, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<Result<TResult>> Send<TResult>(ICqrsQuery<TResult> query, CancellationToken cancellationToken = default)
    {
        var guardResult = ValidateNotNull(query);
        
        if (guardResult.IsFailed)
        {
            return guardResult;
        }

        var queryType = query.GetType();
        var genericType = typeof(ICqrsQueryHandler<,>);

        var closedGenericType = TypeCache.GetOrAdd((genericType, queryType, typeof(TResult)), key =>
            key.GenericType.MakeGenericType(key.MessageType, key.ResultType!));

        var assemblyQualifiedName = closedGenericType.AssemblyQualifiedName ?? "Unknown Assembly";

        using var activity = CreateActivity($"Send {queryType}");

        return await ExecuteHandlerWithResult<TResult>(closedGenericType, query, assemblyQualifiedName, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<Result> Publish<TEvent>(IEnumerable<TEvent> events, CancellationToken cancellationToken = default)
        where TEvent : class, ICqrsEvent
    {
        TEvent[] eventsList = [.. events];

        if (eventsList.Length == 0)
        {
            logger.LogNoEventsReceived();
        }

        foreach (var @event in eventsList)
        {
            await Publish(@event, cancellationToken);
        }

        return Result.Ok();
    }

    /// <inheritdoc />
    public async Task<Result> Publish<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
        where TEvent : class, ICqrsEvent
    {
        var guardResult = ValidateNotNull(@event);

        if (guardResult.IsFailed)
        {
            return guardResult;
        }

        using var activity = CreateActivity($"Publish {@event.GetType()}");

        var assemblyQualifiedName = typeof(ICqrsEventHandler<TEvent>).AssemblyQualifiedName ?? "Unknown Assembly";

        var handlersResult = TryResolveEventHandlers<TEvent>(assemblyQualifiedName);

        if (handlersResult.IsFailed)
        {
            return Result.Fail(handlersResult.Errors); // Convert Result<T> to Result
        }

        var handlers = handlersResult.Value;

        if (handlers.Count == 0)
        {
            LogHandlerNotFound(assemblyQualifiedName);
            return Result.Fail("No handler could be found for this request.");
        }

        var tasks = handlers.Select(handler => handler.Handle(@event, cancellationToken));

        await Task.WhenAll(tasks);

        return Result.Ok();
    }

    private static Result ValidateNotNull(object? obj) 
        => Result.Try(() => Guard.IsNotNull(obj));

    private static Activity? CreateActivity(string activityName)
        => ActivitySource.StartActivity(activityName);

    private Result<IEnumerable<ICqrsCommandHandler<TCommand>>> TryResolveCommandHandlers<TCommand>(string assemblyQualifiedName)
        where TCommand : class, ICqrsCommand
    {
        try
        {
            lifetimeScope.TryResolve(out IEnumerable<ICqrsCommandHandler<TCommand>>? handlers);
            
            return Result.Ok(handlers ?? []);
        }
        catch (DependencyResolutionException ex)
        {
            LogDependencyResolutionError(ex, assemblyQualifiedName);
            
            return Result.Fail("Failed to resolve dependency from the DI container. Check your component is registered.");
        }
    }

    private Result<IReadOnlyList<ICqrsEventHandler<TEvent>>> TryResolveEventHandlers<TEvent>(string assemblyQualifiedName)
        where TEvent : class, ICqrsEvent
    {
        try
        {
            var handlers = lifetimeScope.Resolve<IEnumerable<ICqrsEventHandler<TEvent>>>().ToList();

            return Result.Ok<IReadOnlyList<ICqrsEventHandler<TEvent>>>(handlers);
        }
        catch (ComponentNotRegisteredException ex)
        {
            LogComponentNotRegistered(ex, assemblyQualifiedName);

            return Result.Fail("A component is not registered in the DI container. Check your component is registered.");
        }
        catch (DependencyResolutionException ex)
        {
            LogDependencyResolutionError(ex, assemblyQualifiedName);

            return Result.Fail("Failed to resolve dependency from the DI container. Check your component is registered.");
        }
    }

    private Result ValidateCommandHandlerCount(int handlerCount, string assemblyQualifiedName)
        => handlerCount switch
        {
            0   => HandleNoHandlerFound(assemblyQualifiedName),
            > 1 => HandleMultipleHandlersFound(assemblyQualifiedName),
            _   => Result.Ok()
        };

    private Result HandleNoHandlerFound(string assemblyQualifiedName)
    {
        LogHandlerNotFound(assemblyQualifiedName);
        
        return Result.Fail("No handler could be found for this request.");
    }

    private Result HandleMultipleHandlersFound(string assemblyQualifiedName)
    {
        logger.LogMultipleHandlersFound(assemblyQualifiedName);

        return Result.Fail("Multiple handlers found. A command may only have one handler.");
    }

    private async Task<Result<TResult>> ExecuteHandlerWithResult<TResult>(Type closedGenericType, object message, string assemblyQualifiedName, CancellationToken cancellationToken)
    {
        try
        {
            var handlerExists = lifetimeScope.TryResolve(closedGenericType, out var handler);

            if (!handlerExists)
            {
                LogHandlerNotFound(assemblyQualifiedName);
                return Result.Fail("No handler could be found for this request.");
            }

            var result = await InvokeHandler<TResult>(handler!, message, cancellationToken);
            
            return result;
        }
        catch (DependencyResolutionException ex)
        {
            LogDependencyResolutionError(ex, assemblyQualifiedName);
            
            return Result.Fail("Failed to resolve dependency from the DI container. Check your component is registered.");
        }
        catch (InvalidOperationException ex)
        {
            LogHandlerExecutionError(ex, assemblyQualifiedName);
            
            return Result.Fail("Failed to execute handler.");
        }
    }

    private static async Task<Result<TResult>> InvokeHandler<TResult>(object handler, object message, CancellationToken cancellationToken)
    {
        var handlerType = handler.GetType();
        var messageType = message.GetType();

        var method = MethodCache.GetOrAdd((handlerType, messageType), key
            => key.HandlerType.GetRuntimeMethod("Handle", [key.MessageType, typeof(CancellationToken)]) 
               ?? throw new InvalidOperationException($"Failed to get runtime method 'Handle' from {key.HandlerType}."));

        var resultTask = (Task<Result<TResult>>)method.Invoke(handler, [message, cancellationToken])!;

        return await resultTask;
    }

    private void LogDependencyResolutionError(Exception ex, string assemblyQualifiedName) 
        => logger.LogHandlerResolutionFailure(assemblyQualifiedName, ex);

    private void LogComponentNotRegistered(Exception ex, string assemblyQualifiedName) 
        => logger.LogComponentNotFound(assemblyQualifiedName, ex);

    private void LogHandlerNotFound(string assemblyQualifiedName) 
        => logger.LogHandlerNotFoundCritical(assemblyQualifiedName);

    private void LogHandlerExecutionError(Exception ex, string assemblyQualifiedName) 
        => logger.LogHandlerExecutionFailure(assemblyQualifiedName, ex);
}
