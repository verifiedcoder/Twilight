using System.Diagnostics;
using System.Reflection;
using Autofac;
using Autofac.Core;
using Autofac.Core.Registration;
using Microsoft.Extensions.Logging;
using CommunityToolkit.Diagnostics;
using FluentResults;
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
public sealed class AutofacInMemoryMessageSender : IMessageSender
{
    private const string DefaultAssemblyVersion = "1.0.0.0";

    private readonly ILifetimeScope _lifetimeScope;
    private readonly ILogger<AutofacInMemoryMessageSender> _logger;

    /// <summary>
    ///     Initializes a new instance of the <see cref="AutofacInMemoryMessageSender" /> class.
    /// </summary>
    /// <param name="lifetimeScope">The Autofac lifetime scope.</param>
    /// <param name="logger">The logger.</param>
    public AutofacInMemoryMessageSender(ILifetimeScope lifetimeScope, ILogger<AutofacInMemoryMessageSender> logger)
    {
        Guard.IsNotNull(lifetimeScope);
        Guard.IsNotNull(logger);

        _lifetimeScope = lifetimeScope;
        _logger = logger;
    }

    private static string Namespace => typeof(AutofacInMemoryMessageSender).Namespace ?? nameof(AutofacInMemoryMessageSender);

    private static string AssemblyVersion => Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? DefaultAssemblyVersion;

    /// <inheritdoc />
    public async Task<Result> Send<TCommand>(TCommand command, CancellationToken cancellationToken = default)
        where TCommand : class, ICqrsCommand
    {
        var guardResult = Result.Try(() =>
        {
            Guard.IsNotNull(command);
        });

        if (guardResult.IsFailed)
        {
            return guardResult;
        }

        var activitySource = new ActivitySource(Namespace, AssemblyVersion);

        using var activity = activitySource.StartActivity($"Send {command.GetType()}");
        {
            await using var scope = _lifetimeScope.BeginLifetimeScope();

            var assemblyQualifiedName = typeof(ICqrsCommandHandler<TCommand>).AssemblyQualifiedName ?? "Unknown Assembly";

            IEnumerable<ICqrsCommandHandler<TCommand>>? handlers;

            try
            {
                _lifetimeScope.TryResolve(out handlers);
            }
            catch (DependencyResolutionException ex)
            {
                _logger.LogCritical(ex, $"No concrete handlers for type '{assemblyQualifiedName}' could be resolved.");

                return Result.Fail("Failed to resolve dependency from the DI container. Check your component is registered.");
            }

            var commandHandlers = (handlers ?? Array.Empty<ICqrsCommandHandler<TCommand>>()).ToList();

            switch (commandHandlers.Count)
            {
                case 0:

                    _logger.LogCritical("Handler not found in '{AssemblyQualifiedName}'.", assemblyQualifiedName);

                    return Result.Fail("No handler cold be found for this request.");

                case > 1:

                    _logger.LogCritical("Multiple handlers found in {AssemblyQualifiedName}.", assemblyQualifiedName);

                    return Result.Fail("Multiple handlers found. A command may only have one handler.");

                default:

                    await commandHandlers[0].Handle(command, cancellationToken);
                    break;
            }
        }

        return Result.Ok();
    }

    /// <inheritdoc />
    public async Task<Result<TResult>> Send<TResult>(ICqrsCommand<TResult> command, CancellationToken cancellationToken = default)
    {
        var guardResult = Result.Try(() =>
        {
            Guard.IsNotNull(command);
        });

        if (guardResult.IsFailed)
        {
            return guardResult;
        }

        var genericType = typeof(ICqrsCommandHandler<,>);
        var closedGenericType = genericType.MakeGenericType(command.GetType(), typeof(TResult));
        var assemblyQualifiedName = closedGenericType.AssemblyQualifiedName ?? "Unknown Assembly";

        await using var scope = _lifetimeScope.BeginLifetimeScope();

        var activitySource = new ActivitySource(Namespace, AssemblyVersion);

        using var activity = activitySource.StartActivity($"Send {command.GetType()}");
        {
            object result;

            try
            {
                var handlerExists = _lifetimeScope.TryResolve(closedGenericType, out var handler);

                if (!handlerExists)
                {
                    _logger.LogCritical("Handler not found in '{AssemblyQualifiedName}'.", assemblyQualifiedName);

                    return Result.Fail("No handler cold be found for this request.");
                }

                var handlerType = handler!.GetType();
                var handlerTypeRuntimeMethod = handlerType.GetRuntimeMethod("Handle", [command.GetType(), typeof(CancellationToken)])
                                            ?? throw new InvalidOperationException($"Failed to get runtime method 'Handle' from {handlerType}.");

                var resultTask = (Task<Result<TResult>>)handlerTypeRuntimeMethod.Invoke(handler, [command, cancellationToken])!;

                result = await resultTask;
            }
            catch (DependencyResolutionException ex)
            {
                _logger.LogCritical(ex, $"No concrete handlers for type '{assemblyQualifiedName}' could be resolved.");

                return Result.Fail("Failed to resolve dependency from the DI container. Check your component is registered.");
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogCritical(ex, $"Could not execute handler for '{assemblyQualifiedName}'.");

                return Result.Fail("Failed to execute handler.");
            }

            return (Result<TResult>)result;
        }
    }

    /// <inheritdoc />
    public async Task<Result<TResult>> Send<TResult>(ICqrsQuery<TResult> query, CancellationToken cancellationToken = default)
    {
        var guardResult = Result.Try(() =>
        {
            Guard.IsNotNull(query);
        });

        if (guardResult.IsFailed)
        {
            return guardResult;
        }

        var genericType = typeof(ICqrsQueryHandler<,>);
        var closedGenericType = genericType.MakeGenericType(query.GetType(), typeof(TResult));
        var assemblyQualifiedName = closedGenericType.AssemblyQualifiedName ?? "Unknown Assembly";

        await using var scope = _lifetimeScope.BeginLifetimeScope();

        var activitySource = new ActivitySource(Namespace, AssemblyVersion);

        using var activity = activitySource.StartActivity($"Send {query.GetType()}");
        {
            object result;

            try
            {
                var handlerExists = _lifetimeScope.TryResolve(closedGenericType, out var handler);

                if (!handlerExists)
                {
                    _logger.LogCritical("Handler not found in '{AssemblyQualifiedName}'.", assemblyQualifiedName);

                    return Result.Fail("No handler cold be found for this request.");
                }

                var handlerType = handler!.GetType();
                var handlerTypeRuntimeMethod = handlerType.GetRuntimeMethod("Handle", [query.GetType(), typeof(CancellationToken)])
                                            ?? throw new InvalidOperationException($"Failed to get runtime method 'Handle' from {handlerType}.");

                var resultTask = (Task<Result<TResult>>)handlerTypeRuntimeMethod.Invoke(handler, [query, cancellationToken])!;

                result = await resultTask;
            }
            catch (DependencyResolutionException ex)
            {
                _logger.LogCritical(ex, $"No concrete handlers for type '{assemblyQualifiedName}' could be resolved.");

                return Result.Fail("Failed to resolve dependency from the DI container. Check your component is registered.");
            }

            return (Result<TResult>)result;
        }
    }

    /// <inheritdoc />
    public async Task<Result> Publish<TEvent>(IEnumerable<TEvent> events, CancellationToken cancellationToken = default)
        where TEvent : class, ICqrsEvent
    {
        var eventsList = events as TEvent[] ?? events.ToArray();

        if (!eventsList.Length.Equals(0))
        {
            _logger.LogWarning("No events received for publishing when at least one event was expected. Check calls to publish.");
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
        var guardResult = Result.Try(() =>
        {
            Guard.IsNotNull(@event);
        });

        if (guardResult.IsFailed)
        {
            return guardResult;
        }

        var activitySource = new ActivitySource(Namespace, AssemblyVersion);

        using var activity = activitySource.StartActivity($"Publish {@event.GetType()}");
        {
            await using var scope = _lifetimeScope.BeginLifetimeScope();

            var assemblyQualifiedName = typeof(ICqrsEventHandler<TEvent>).AssemblyQualifiedName ?? "Unknown Assembly";

            IEnumerable<ICqrsEventHandler<TEvent>> handlers;

            try
            {
                handlers = _lifetimeScope.Resolve<IEnumerable<ICqrsEventHandler<TEvent>>>()
                                         .ToList();
            }
            catch (ComponentNotRegisteredException ex)
            {
                _logger.LogCritical(ex, $"No concrete handlers for type '{assemblyQualifiedName}' could be found.");

                return Result.Fail("A component is not registered in the DI container. Check your component is registered.");
            }
            catch (DependencyResolutionException ex)
            {
                _logger.LogCritical(ex, $"No concrete handlers for type '{assemblyQualifiedName}' could be resolved.");

                return Result.Fail("Failed to resolve dependency from the DI container. Check your component is registered.");
            }

            if (!handlers.Any())
            {
                _logger.LogCritical("Handler not found in '{AssemblyQualifiedName}'.", assemblyQualifiedName);

                return Result.Fail("No handler cold be found for this request.");
            }

            var tasks = new List<Task>(handlers.Count());

            tasks.AddRange(handlers.Select(handler => handler.Handle(@event, cancellationToken)));

            await Task.WhenAll(tasks);
        }

        return Result.Ok();
    }
}
