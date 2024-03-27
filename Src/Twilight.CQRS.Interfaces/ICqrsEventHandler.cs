using FluentResults;

namespace Twilight.CQRS.Interfaces;

/// <summary>
///     Represents a means of handling an event in order to broker a result.
/// </summary>
/// <typeparam name="TEvent">The type of the event.</typeparam>
public interface ICqrsEventHandler<in TEvent> : ICqrsMessageHandler<TEvent>
    where TEvent : ICqrsEvent
{
    /// <summary>
    ///     Handles the event.
    /// </summary>
    /// <param name="event">The event.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous event handler operation.</returns>
    Task<Result> Handle(TEvent @event, CancellationToken cancellationToken = default);
}
