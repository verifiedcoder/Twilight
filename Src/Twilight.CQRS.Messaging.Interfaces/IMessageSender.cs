using Twilight.CQRS.Interfaces;

namespace Twilight.CQRS.Messaging.Interfaces;

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
    Task<Result> Send<TCommand>(TCommand command, CancellationToken cancellationToken = default)
        where TCommand : class, ICqrsCommand;

    /// <summary>
    ///     Runs the command handler registered for the given command type.
    /// </summary>
    /// <remarks>
    ///     This method should be implemented when a response (reply) to the originating service is required (i.e. the
    ///     result of the command is fulfilled). It is recommended to restrain a command response to a scalar value.
    /// </remarks>
    /// <typeparam name="TResponse">Type of the result.</typeparam>
    /// <param name="command">Instance of the command.</param>
    /// <param name="cancellationToken">Task cancellation token.</param>
    /// <returns>A Task that resolves to a result of the command handler.</returns>
    Task<Result<TResponse>> Send<TResponse>(ICqrsCommand<TResponse> command, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Runs the query handler registered for the given query type.
    /// </summary>
    /// <remarks>
    ///     This method should be implemented when a response (reply) to the originating service is required (i.e. the
    ///     result of the query is fulfilled).
    /// </remarks>
    /// <typeparam name="TResponse">Type of the result.</typeparam>
    /// <param name="query">Instance of the query.</param>
    /// <param name="cancellationToken">Task cancellation token.</param>
    /// <returns>A Task that resolves to a result of the query handler.</returns>
    Task<Result<TResponse>> Send<TResponse>(ICqrsQuery<TResponse> query, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Runs all registered event handlers for the specified events.
    /// </summary>
    /// <param name="events">The domain events.</param>
    /// <param name="cancellationToken">Task cancellation token.</param>
    /// <returns>Task that completes when all handlers finish processing.</returns>
    Task<Result> Publish<TEvent>(IEnumerable<TEvent> events, CancellationToken cancellationToken = default)
        where TEvent : class, ICqrsEvent;

    /// <summary>
    ///     Runs all registered event handlers for the specified event.
    /// </summary>
    /// <typeparam name="TEvent">Type of the event.</typeparam>
    /// <param name="event">Instance of the event.</param>
    /// <param name="cancellationToken">Task cancellation token.</param>
    /// <returns>A Task that completes when all handlers finish processing.</returns>
    Task<Result> Publish<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
        where TEvent : class, ICqrsEvent;
}
