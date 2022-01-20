using System.Diagnostics;
using System.Reflection;
using Autofac;
using Autofac.Core;
using Autofac.Core.Registration;
using Microsoft.Extensions.Logging;
using Microsoft.Toolkit.Diagnostics;
using Twilight.CQRS.Interfaces;
using Twilight.CQRS.Messaging.Common;
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
        Guard.IsNotNull(lifetimeScope, nameof(lifetimeScope));
        Guard.IsNotNull(logger, nameof(logger));

        _lifetimeScope = lifetimeScope;
        _logger = logger;
    }

    private static string Namespace => typeof(AutofacInMemoryMessageSender).Namespace ?? nameof(AutofacInMemoryMessageSender);

    private static string AssemblyVersion => Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? DefaultAssemblyVersion;

    /// <inheritdoc />
    /// <exception cref="MultipleCommandHandlersDefinedException">
    ///     Thrown when more than one cqrsCommand handler is resolved from the container.
    /// </exception>
    /// <exception cref="HandlerNotFoundException">Thrown when a cqrsCommand handler cannot be resolved from the container.</exception>
    /// <exception cref="MultipleCommandHandlersDefinedException">
    ///     Thrown when more than one cqrsCommand handler with the same
    ///     definition is found.
    /// </exception>
    public async Task Send<TCommand>(TCommand cqrsCommand, CancellationToken cancellationToken = default)
        where TCommand : class, ICqrsCommand
    {
        Guard.IsNotNull(cqrsCommand, nameof(cqrsCommand));

        var activitySource = new ActivitySource(Namespace, AssemblyVersion);

        using var activity = activitySource.StartActivity($"Send {cqrsCommand.GetType()}");
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
                _logger.LogCritical(ex, "Dependency Resolution Exception");

                throw new HandlerNotFoundException(assemblyQualifiedName, ex);
            }

            var commandHandlers = (handlers ?? Array.Empty<ICqrsCommandHandler<TCommand>>()).ToList();

            switch (commandHandlers.Count)
            {
                case 0:

                    _logger.LogCritical("Handler not found in {AssemblyQualifiedName}.", assemblyQualifiedName);

                    throw new HandlerNotFoundException(assemblyQualifiedName);

                case > 1:

                    _logger.LogCritical("Multiple handlers found in {AssemblyQualifiedName}.", assemblyQualifiedName);

                    throw new MultipleCommandHandlersDefinedException(assemblyQualifiedName);

                default:

                    await commandHandlers.First().Handle(cqrsCommand, cancellationToken);
                    break;
            }
        }
    }

    /// <inheritdoc />
    /// <exception cref="MultipleCommandHandlersDefinedException">
    ///     Thrown when more than one cqrsCommand handler is resolved from the container.
    /// </exception>
    /// <exception cref="HandlerNotFoundException">Thrown when a command handler cannot be resolved from the container.</exception>
    public async Task<TResult> Send<TResult>(ICqrsCommand<TResult> cqrsCommand, CancellationToken cancellationToken = default)
    {
        Guard.IsNotNull(cqrsCommand, nameof(cqrsCommand));

        var genericType = typeof(ICqrsCommandHandler<,>);
        var closedGenericType = genericType.MakeGenericType(cqrsCommand.GetType(), typeof(TResult));
        var assemblyQualifiedName = closedGenericType.AssemblyQualifiedName ?? "Unknown Assembly";

        await using var scope = _lifetimeScope.BeginLifetimeScope();

        var activitySource = new ActivitySource(Namespace, AssemblyVersion);

        using var activity = activitySource.StartActivity($"Send {cqrsCommand.GetType()}");
        {
            object result;

            try
            {
                var handlerExists = _lifetimeScope.TryResolve(closedGenericType, out var handler);

                if (!handlerExists || handler == null)
                {
                    _logger.LogCritical("Handler not found in {AssemblyQualifiedName}.", assemblyQualifiedName);

                    throw new HandlerNotFoundException(assemblyQualifiedName);
                }

                var handlerType = handler.GetType();
                var handlerTypeRuntimeMethod = handlerType.GetRuntimeMethod("Handle", new[] { cqrsCommand.GetType(), typeof(CancellationToken) })
                                            ?? throw new InvalidOperationException($"Failed to get runtime method 'Handle' from {handlerType}.");

                result = handlerTypeRuntimeMethod.Invoke(handler, new object[] { cqrsCommand, cancellationToken }) ?? throw new InvalidOperationException();
            }
            catch (DependencyResolutionException ex)
            {
                _logger.LogCritical(ex, "Handler not found in {AssemblyQualifiedName}.", assemblyQualifiedName);

                throw new HandlerNotFoundException(assemblyQualifiedName, ex);
            }

            return await (Task<TResult>)result;
        }
    }

    /// <inheritdoc />
    /// <exception cref="HandlerNotFoundException">Thrown when a query handler cannot be resolved from the container.</exception>
    /// <exception cref="InvalidOperationException">Thrown when a handler type cannot resolved the type's 'Handle' method.</exception>
    public async Task<TResult> Send<TResult>(ICqrsQuery<TResult> cqrsQuery, CancellationToken cancellationToken = default)
    {
        Guard.IsNotNull(cqrsQuery, nameof(cqrsQuery));

        var genericType = typeof(ICqrsQueryHandler<,>);
        var closedGenericType = genericType.MakeGenericType(cqrsQuery.GetType(), typeof(TResult));
        var assemblyQualifiedName = closedGenericType.AssemblyQualifiedName ?? "Unknown Assembly";

        await using var scope = _lifetimeScope.BeginLifetimeScope();

        var activitySource = new ActivitySource(Namespace, AssemblyVersion);

        using var activity = activitySource.StartActivity($"Send {cqrsQuery.GetType()}");
        {
            object result;

            try
            {
                var handlerExists = _lifetimeScope.TryResolve(closedGenericType, out var handler);

                if (!handlerExists || handler == null)
                {
                    _logger.LogCritical("Handler not found in {AssemblyQualifiedName}.", assemblyQualifiedName);

                    throw new HandlerNotFoundException(assemblyQualifiedName);
                }

                var handlerType = handler.GetType();
                var handlerTypeRuntimeMethod = handlerType.GetRuntimeMethod("Handle", new[] { cqrsQuery.GetType(), typeof(CancellationToken) })
                                            ?? throw new InvalidOperationException($"Failed to get runtime method 'Handle' from {handlerType}.");

                result = handlerTypeRuntimeMethod.Invoke(handler, new object[] { cqrsQuery, cancellationToken }) ?? throw new InvalidOperationException();
            }
            catch (DependencyResolutionException ex)
            {
                _logger.LogCritical(ex, "Handler not found in {AssemblyQualifiedName}.", assemblyQualifiedName);

                throw new HandlerNotFoundException(assemblyQualifiedName, ex);
            }

            return await (Task<TResult>)result;
        }
    }

    /// <inheritdoc />
    public async Task Publish<TEvent>(IEnumerable<TEvent> events, CancellationToken cancellationToken = default)
        where TEvent : class, ICqrsEvent
    {
        Guard.IsNotNull(events, nameof(events));

        var enumerable = events as TEvent[] ?? events.ToArray();

        if (!enumerable.Length.Equals(0))
        {
            _logger.LogWarning("No events received for publishing when at least one event was expected. Check calls to publish.");
        }

        foreach (var @event in enumerable)
        {
            await Publish(@event, cancellationToken);
        }
    }

    /// <inheritdoc />
    /// <exception cref="HandlerNotFoundException">Thrown when a cqrsQuery handler cannot be resolved from the container.</exception>
    public async Task Publish<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
        where TEvent : class, ICqrsEvent
    {
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
                _logger.LogCritical(ex, "A component is not registered in the DI container. Check your component is registered.");

                throw new HandlerNotFoundException(assemblyQualifiedName, ex);
            }
            catch (DependencyResolutionException ex)
            {
                _logger.LogCritical(ex, "Failed to resolve dependency from the DI container. Check your component is registered.");

                throw new HandlerNotFoundException(assemblyQualifiedName, ex);
            }

            if (!handlers.Any())
            {
                _logger.LogCritical("Handler not found in {AssemblyQualifiedName}.", assemblyQualifiedName);

                throw new HandlerNotFoundException(assemblyQualifiedName);
            }

            var tasks = new List<Task>(handlers.Count());

            tasks.AddRange(handlers.Select(handler => handler.Handle(@event, cancellationToken)));

            await Task.WhenAll(tasks);
        }
    }
}
