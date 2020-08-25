using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Twilight.CQRS.Contracts;

namespace Twilight.CQRS.Events
{
    /// <summary>
    ///     <para>
    ///         Represents the ability to process (handle) events. An event handler receives a <em>published</em> event and
    ///         brokers a result. A result is either a successful consumption of the event, or an exception. Events can be
    ///         consumed by multiple event handlers. This class cannot be instantiated.
    ///     </para>
    ///     <para>Implements <see cref="MessageHandler{TEvent}" />.</para>
    ///     <para>Implements <see cref="IEventHandler{TEvent}" />.</para>
    /// </summary>
    /// <typeparam name="TEvent">The type of the event.</typeparam>
    /// <seealso cref="MessageHandler{TEvent}" />
    /// <seealso cref="IEventHandler{TEvent}" />
    public abstract class EventHandlerBase<TEvent> : MessageHandler<TEvent>, IEventHandler<TEvent>
        where TEvent : class, IEvent
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="EventHandlerBase{TEvent}" /> class.
        /// </summary>
        /// <param name="validator">The event validator.</param>
        /// <param name="logger">The logger.</param>
        protected EventHandlerBase(ILogger<IMessageHandler<TEvent>> logger, IValidator<TEvent>? validator = default)
            : base(logger, validator)
        {
        }

        /// <inheritdoc />
        public async Task Handle(TEvent @event, CancellationToken cancellationToken = default)
        {
            await OnBeforeHandling(@event, cancellationToken);

            await ValidateMessage(@event, cancellationToken);

            await HandleEvent(@event, cancellationToken);

            await OnAfterHandling(@event, cancellationToken);
        }

        /// <summary>
        ///     Handles the event.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous handle event operation.</returns>
        protected abstract Task HandleEvent(TEvent @event, CancellationToken cancellationToken = default);
    }
}
