using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
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
        protected EventHandlerBase(IValidator<TEvent>? validator = default)
            : base(validator)
        {
        }

        /// <inheritdoc />
        public async Task Handle(TEvent @event, CancellationToken cancellationToken = default)
        {
            var activitySource = new ActivitySource(ActivitySourceName, AssemblyVersion);

            using var activity = activitySource.StartActivity($"Handle {@event.GetType()}");
            {
                using (var childSpan = activitySource.StartActivity("OnBeforeHandlingEvent", ActivityKind.Consumer))
                {
                    childSpan?.AddEvent(new ActivityEvent($"{nameof(EventHandlerBase<TEvent>)}.{nameof(OnBeforeHandling)}"));

                    await OnBeforeHandling(@event, cancellationToken);
                }

                using (var childSpan = activitySource.StartActivity("ValidateEvent", ActivityKind.Consumer))
                {
                    childSpan?.AddEvent(new ActivityEvent($"{nameof(EventHandlerBase<TEvent>)}.{nameof(ValidateMessage)}"));

                    await ValidateMessage(@event, cancellationToken);
                }

                using (var childSpan = activitySource.StartActivity("HandleEvent", ActivityKind.Consumer))
                {
                    childSpan?.AddEvent(new ActivityEvent($"{nameof(EventHandlerBase<TEvent>)}.{nameof(HandleEvent)}"));

                    await HandleEvent(@event, cancellationToken);
                }

                using (var childSpan = activitySource.StartActivity("OnAfterHandlingEvent", ActivityKind.Consumer))
                {
                    childSpan?.AddEvent(new ActivityEvent($"{nameof(EventHandlerBase<TEvent>)}.{nameof(OnAfterHandling)}"));

                    await OnAfterHandling(@event, cancellationToken);
                }
            }
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
