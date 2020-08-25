using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Twilight.CQRS.Contracts;

namespace Twilight.CQRS.Messaging.Contracts
{
    /// <summary>
    ///     Represents a means of dispatching messages.
    /// </summary>
    public interface IMessageSender
    {
        /// <summary>
        ///     Runs the command handler registered for the given command type.
        /// </summary>
        /// <typeparam name="TCommand">Type of the command.</typeparam>
        /// <param name="command">Instance of the command.</param>
        /// <param name="cancellationToken">Task cancellation token.</param>
        /// <returns>A Task that completes when the handler finished processing.</returns>
        Task Send<TCommand>(TCommand command, CancellationToken cancellationToken = default)
            where TCommand : ICommand;

        /// <summary>
        ///     Runs the query handler registered for the given query type.
        /// </summary>
        /// <remarks>
        ///     This method should be implemented when a response (reply) to the originating service is required (i.e. the
        ///     result of the query is fulfilled).
        /// </remarks>
        /// <typeparam name="TResult">Type of the query.</typeparam>
        /// <param name="query">Instance of the query.</param>
        /// <param name="cancellationToken">Task cancellation token.</param>
        /// <returns>A Task that resolves to a result of the query handler.</returns>
        Task<TResult> Send<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Runs all registered event handlers for the specified events.
        /// </summary>
        /// <param name="events">The domain events.</param>
        /// <param name="cancellationToken">Task cancellation token.</param>
        /// <returns>Task that completes when all handlers finish processing.</returns>
        Task Publish<TEvent>(IEnumerable<TEvent> events, CancellationToken cancellationToken)
            where TEvent : IEvent;

        /// <summary>
        ///     Runs all registered event handlers for the specified event.
        /// </summary>
        /// <typeparam name="TEvent">Type of the event.</typeparam>
        /// <param name="event">Instance of the event.</param>
        /// <param name="cancellationToken">Task cancellation token.</param>
        /// <returns>A Task that completes when all handlers finish processing.</returns>
        Task Publish<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
            where TEvent : IEvent;
    }
}
