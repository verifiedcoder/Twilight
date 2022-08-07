using System.Diagnostics;
using FluentValidation;
using Microsoft.Extensions.Logging;
using CommunityToolkit.Diagnostics;
using Twilight.CQRS.Interfaces;
// ReSharper disable ExplicitCallerInfoArgument as false positive for StartActivity

namespace Twilight.CQRS.Events;

/// <summary>
///     <para>
///         Represents the ability to process (handle) events. An event handler receives a <em>published</em> event and
///         brokers a result. A result is either a successful consumption of the event, or an exception. Events can be
///         consumed by multiple event handlers. This class cannot be instantiated.
///     </para>
///     <para>Implements <see cref="CqrsMessageHandler{THandler,TMessage}" />.</para>
///     <para>Implements <see cref="ICqrsEventHandler{TEvent}" />.</para>
/// </summary>
/// <typeparam name="TEvent">The type of the event.</typeparam>
/// <typeparam name="TEventHandler">The type of the event handler.</typeparam>
/// <seealso cref="CqrsMessageHandler{THandler,TMessage}" />
/// <seealso cref="ICqrsEventHandler{TEvent}" />
public abstract class CqrsEventHandlerBase<TEventHandler, TEvent> : CqrsMessageHandler<TEventHandler, TEvent>, ICqrsEventHandler<TEvent>
    where TEvent : class, ICqrsEvent
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="CqrsEventHandlerBase{TEventHandler,TEvent}" /> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="validator">The event validator.</param>
    protected CqrsEventHandlerBase(ILogger<TEventHandler> logger, IValidator<TEvent>? validator = default)
        : base(logger, validator)
    {
    }

    /// <inheritdoc />
    public async Task Handle(TEvent @event, CancellationToken cancellationToken = default)
    {
        Guard.IsNotNull(@event, nameof(@event));

        using var activity = Activity.Current?.Source.StartActivity($"Handle {@event.GetType()}");
        {
            using (var childSpan = Activity.Current?.Source.StartActivity("Pre event handling logic"))
            {
                childSpan?.AddEvent(new ActivityEvent($"{nameof(CqrsEventHandlerBase<TEventHandler, TEvent>)}.{nameof(OnBeforeHandling)}"));

                await OnBeforeHandling(@event, cancellationToken);
            }

            using (var childSpan = Activity.Current?.Source.StartActivity("Validate event"))
            {
                childSpan?.AddEvent(new ActivityEvent($"{nameof(CqrsEventHandlerBase<TEventHandler, TEvent>)}.{nameof(ValidateMessage)}"));

                await ValidateMessage(@event, cancellationToken);
            }

            using (var childSpan = Activity.Current?.Source.StartActivity("Handle event"))
            {
                childSpan?.AddEvent(new ActivityEvent($"{nameof(CqrsEventHandlerBase<TEventHandler, TEvent>)}.{nameof(HandleEvent)}"));

                await HandleEvent(@event, cancellationToken);
            }

            using (var childSpan = Activity.Current?.Source.StartActivity("Post event handling logic"))
            {
                childSpan?.AddEvent(new ActivityEvent($"{nameof(CqrsEventHandlerBase<TEventHandler, TEvent>)}.{nameof(OnAfterHandling)}"));

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
    public abstract Task HandleEvent(TEvent @event, CancellationToken cancellationToken = default);
}
