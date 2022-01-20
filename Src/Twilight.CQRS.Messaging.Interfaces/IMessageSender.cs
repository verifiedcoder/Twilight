using Twilight.CQRS.Interfaces;

namespace Twilight.CQRS.Messaging.Interfaces;

/// <summary>
///     Represents a means of dispatching messages.
/// </summary>
public interface IMessageSender
{
    /// <summary>
    ///     Runs the cqrsCommand handler registered for the given cqrsCommand type.
    /// </summary>
    /// <typeparam name="TCommand">Type of the cqrsCommand.</typeparam>
    /// <param name="cqrsCommand">Instance of the cqrsCommand.</param>
    /// <param name="cancellationToken">Task cancellation token.</param>
    /// <returns>A Task that completes when the handler finished processing.</returns>
    Task Send<TCommand>(TCommand cqrsCommand, CancellationToken cancellationToken = default)
        where TCommand : class, ICqrsCommand;

    /// <summary>
    ///     Runs the cqrsCommand handler registered for the given cqrsCommand type.
    /// </summary>
    /// <remarks>
    ///     This method should be implemented when a response (reply) to the originating service is required (i.e. the
    ///     result of the cqrsCommand is fulfilled). It is recommended to restrain a cqrsCommand response to a scalar value.
    /// </remarks>
    /// <typeparam name="TResponse">Type of the result.</typeparam>
    /// <param name="cqrsCommand">Instance of the cqrsCommand.</param>
    /// <param name="cancellationToken">Task cancellation token.</param>
    /// <returns>A Task that resolves to a result of the cqrsCommand handler.</returns>
    Task<TResponse> Send<TResponse>(ICqrsCommand<TResponse> cqrsCommand, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Runs the cqrsQuery handler registered for the given cqrsQuery type.
    /// </summary>
    /// <remarks>
    ///     This method should be implemented when a response (reply) to the originating service is required (i.e. the
    ///     result of the cqrsQuery is fulfilled).
    /// </remarks>
    /// <typeparam name="TResponse">Type of the result.</typeparam>
    /// <param name="cqrsQuery">Instance of the cqrsQuery.</param>
    /// <param name="cancellationToken">Task cancellation token.</param>
    /// <returns>A Task that resolves to a result of the cqrsQuery handler.</returns>
    Task<TResponse> Send<TResponse>(ICqrsQuery<TResponse> cqrsQuery, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Runs all registered event handlers for the specified events.
    /// </summary>
    /// <param name="events">The domain events.</param>
    /// <param name="cancellationToken">Task cancellation token.</param>
    /// <returns>Task that completes when all handlers finish processing.</returns>
    Task Publish<TEvent>(IEnumerable<TEvent> events, CancellationToken cancellationToken = default)
        where TEvent : class, ICqrsEvent;

    /// <summary>
    ///     Runs all registered event handlers for the specified event.
    /// </summary>
    /// <typeparam name="TEvent">Type of the event.</typeparam>
    /// <param name="event">Instance of the event.</param>
    /// <param name="cancellationToken">Task cancellation token.</param>
    /// <returns>A Task that completes when all handlers finish processing.</returns>
    Task Publish<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
        where TEvent : class, ICqrsEvent;
}
