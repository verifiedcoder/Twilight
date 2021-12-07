﻿using System.Diagnostics;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Microsoft.Toolkit.Diagnostics;
using Twilight.CQRS.Interfaces;

namespace Twilight.CQRS.Events;

/// <summary>
///     <para>
///         Represents the ability to process (handle) events. An event handler receives a <em>published</em> event and
///         brokers a result. A result is either a successful consumption of the event, or an exception. Events can be
///         consumed by multiple event handlers. This class cannot be instantiated.
///     </para>
///     <para>Implements <see cref="MessageHandler{TEventHandler, TEvent}" />.</para>
///     <para>Implements <see cref="IEventHandler{TEvent}" />.</para>
/// </summary>
/// <typeparam name="TEvent">The type of the event.</typeparam>
/// <typeparam name="TEventHandler">The type of the event handler.</typeparam>
/// <seealso cref="MessageHandler{TEventHandler, TEvent}" />
/// <seealso cref="IEventHandler{TEvent}" />
public abstract class EventHandlerBase<TEventHandler, TEvent> : MessageHandler<TEventHandler, TEvent>, IEventHandler<TEvent>
    where TEvent : class, IEvent
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="EventHandlerBase{TEventHandler, TEvent}" /> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="validator">The event validator.</param>
    protected EventHandlerBase(ILogger<TEventHandler> logger, IValidator<TEvent>? validator = default)
        : base(logger, validator)
    {
    }

    /// <inheritdoc />
    public async Task Handle(TEvent @event, CancellationToken cancellationToken = default)
    {
        Guard.IsNotNull(@event, nameof(@event));

        var activitySource = new ActivitySource(ActivitySourceName, AssemblyVersion);

        using var activity = activitySource.StartActivity($"Handle {@event.GetType()}");
        {
            using (var childSpan = activitySource.StartActivity("OnBeforeHandlingEvent", ActivityKind.Consumer))
            {
                childSpan?.AddEvent(new ActivityEvent($"{nameof(EventHandlerBase<TEventHandler, TEvent>)}.{nameof(OnBeforeHandling)}"));

                await OnBeforeHandling(@event, cancellationToken);
            }

            using (var childSpan = activitySource.StartActivity("ValidateEvent", ActivityKind.Consumer))
            {
                childSpan?.AddEvent(new ActivityEvent($"{nameof(EventHandlerBase<TEventHandler, TEvent>)}.{nameof(ValidateMessage)}"));

                await ValidateMessage(@event, cancellationToken);
            }

            using (var childSpan = activitySource.StartActivity("HandleEvent", ActivityKind.Consumer))
            {
                childSpan?.AddEvent(new ActivityEvent($"{nameof(EventHandlerBase<TEventHandler, TEvent>)}.{nameof(HandleEvent)}"));

                await HandleEvent(@event, cancellationToken);
            }

            using (var childSpan = activitySource.StartActivity("OnAfterHandlingEvent", ActivityKind.Consumer))
            {
                childSpan?.AddEvent(new ActivityEvent($"{nameof(EventHandlerBase<TEventHandler, TEvent>)}.{nameof(OnAfterHandling)}"));

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

