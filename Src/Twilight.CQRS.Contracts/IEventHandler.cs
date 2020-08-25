using System.Threading;
using System.Threading.Tasks;

namespace Twilight.CQRS.Contracts
{
    /// <summary>
    ///     Represents a means of handling an event in order to broker a result.
    /// </summary>
    /// <typeparam name="TEvent">The type of the event.</typeparam>
    public interface IEventHandler<in TEvent> : IMessageHandler<TEvent>
        where TEvent : IEvent
    {
        /// <summary>
        ///     Handles the event.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous event handler operation.</returns>
        Task Handle(TEvent @event, CancellationToken cancellationToken);
    }
}
