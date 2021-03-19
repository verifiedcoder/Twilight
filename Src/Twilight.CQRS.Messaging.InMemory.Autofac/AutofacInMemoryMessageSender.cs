using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;
using Autofac.Core.Registration;
using Microsoft.Extensions.Logging;
using Twilight.CQRS.Contracts;
using Twilight.CQRS.Messaging.Contracts;
using Twilight.CQRS.Messaging.Shared;

namespace Twilight.CQRS.Messaging.InMemory.Autofac
{
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
        private readonly ILifetimeScope _lifetimeScope;
        private readonly ILogger<AutofacInMemoryMessageSender> _logger;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AutofacInMemoryMessageSender" /> class.
        /// </summary>
        /// <param name="lifetimeScope">The Autofac lifetime scope.</param>
        /// <param name="logger">The logger.</param>
        public AutofacInMemoryMessageSender(ILifetimeScope lifetimeScope, ILogger<AutofacInMemoryMessageSender> logger)
        {
            _lifetimeScope = lifetimeScope;
            _logger = logger;
        }

        private static string AssemblyVersion => Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "1.0.0.0";
        private static string Namespace => typeof(AutofacInMemoryMessageSender).Namespace ?? nameof(AutofacInMemoryMessageSender);

        /// <exception cref="MultipleCommandHandlersDefinedException">
        ///     Thrown when more than one command handler is resolved from the container.
        /// </exception>
        /// <exception cref="HandlerNotFoundException">Thrown when a command handler cannot be resolved from the container.</exception>
        /// <inheritdoc />
        public async Task Send<TCommand>(TCommand command, CancellationToken cancellationToken = default)
            where TCommand : ICommand
        {
            var activitySource = new ActivitySource(Namespace, AssemblyVersion);

            using var activity = activitySource.StartActivity($"Send {command.GetType()}");
            {
                await using var scope = _lifetimeScope.BeginLifetimeScope();

                var assemblyQualifiedName = typeof(ICommandHandler<TCommand>).AssemblyQualifiedName ?? "Unknown Assembly";

                IEnumerable<ICommandHandler<TCommand>>? handlers;

                try
                {
                    _lifetimeScope.TryResolve(out handlers);
                }
                catch (DependencyResolutionException ex)
                {
                    _logger.LogError(ex, "Handler not found in {AssemblyQualifiedName}.", assemblyQualifiedName);
                    throw new HandlerNotFoundException(assemblyQualifiedName, ex);
                }

                var commandHandlers = (handlers ?? Array.Empty<ICommandHandler<TCommand>>()).ToList();

                switch (commandHandlers.Count)
                {
                    case 0:

                        _logger.LogError("Handler not found in {AssemblyQualifiedName}.", assemblyQualifiedName);

                        throw new HandlerNotFoundException(assemblyQualifiedName);

                    case > 1:

                        _logger.LogError("Multiple handlers found in {AssemblyQualifiedName}.", assemblyQualifiedName);

                        throw new MultipleCommandHandlersDefinedException(assemblyQualifiedName);

                    default:

                        await commandHandlers.First().Handle(command, cancellationToken).ConfigureAwait(false);
                        break;
                }
            }
        }

        /// <exception cref="HandlerNotFoundException">Thrown when a query handler cannot be resolved from the container.</exception>
        /// <exception cref="InvalidOperationException">Thrown when a handler type cannot resolved the type's 'Handle' method.</exception>
        /// <inheritdoc />
        public async Task<TResult> Send<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default)
        {
            var genericType = typeof(IQueryHandler<,>);
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

                    if (!handlerExists || handler == null)
                    {
                        _logger.LogError("Handler not found in {AssemblyQualifiedName}.", assemblyQualifiedName);
                        throw new HandlerNotFoundException(assemblyQualifiedName);
                    }

                    var handlerType = handler.GetType();
                    var handlerTypeRuntimeMethod = handlerType.GetRuntimeMethod("Handle", new[] {query.GetType(), typeof(CancellationToken)})
                                                ?? throw new InvalidOperationException($"Failed to get runtime method 'Handle' from {handlerType}.");

                    result = handlerTypeRuntimeMethod.Invoke(handler, new object[] {query, cancellationToken}) ?? throw new InvalidOperationException();
                }
                catch (DependencyResolutionException ex)
                {
                    _logger.LogError(ex, "Handler not found in {AssemblyQualifiedName}.", assemblyQualifiedName);
                    throw new HandlerNotFoundException(assemblyQualifiedName, ex);
                }

                return await (Task<TResult>) result;
            }
        }

        /// <inheritdoc />
        public async Task Publish<TEvent>(IEnumerable<TEvent> events, CancellationToken cancellationToken = default)
            where TEvent : IEvent
        {
            var enumerable = events as TEvent[] ?? events.ToArray();

            if (!enumerable.Length.Equals(0))
            {
                _logger.LogCritical("No events received for publishing when at least one event was expected. Check calls to publish.");
            }

            foreach (var @event in enumerable)
            {
                await Publish(@event, cancellationToken);
            }
        }

        /// <exception cref="HandlerNotFoundException">Thrown when a query handler cannot be resolved from the container.</exception>
        /// <inheritdoc />
        public async Task Publish<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
            where TEvent : IEvent
        {
            var activitySource = new ActivitySource(Namespace, AssemblyVersion);

            using var activity = activitySource.StartActivity($"Publish {@event.GetType()}");
            {
                await using var scope = _lifetimeScope.BeginLifetimeScope();

                var assemblyQualifiedName = typeof(IEventHandler<TEvent>).AssemblyQualifiedName ?? "Unknown Assembly";

                IEnumerable<IEventHandler<TEvent>> handlers;

                try
                {
                    handlers = _lifetimeScope.Resolve<IEnumerable<IEventHandler<TEvent>>>()
                                             .ToList();
                }
                catch (ComponentNotRegisteredException ex)
                {
                    _logger.LogError(ex, ex.Message);

                    throw new HandlerNotFoundException(assemblyQualifiedName, ex);
                }
                catch (DependencyResolutionException ex)
                {
                    _logger.LogError(ex, ex.Message);

                    throw new HandlerNotFoundException(assemblyQualifiedName, ex);
                }

                if (!handlers.Any())
                {
                    _logger.LogError("Handler not found in {AssemblyQualifiedName}.", assemblyQualifiedName);

                    throw new HandlerNotFoundException(assemblyQualifiedName);
                }

                var tasks = new List<Task>(handlers.Count());

                tasks.AddRange(handlers.Select(handler => handler.Handle(@event, cancellationToken)));

                await Task.WhenAll(tasks);
            }
        }
    }
}
