using CommunityToolkit.Diagnostics;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
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
/// <param name="logger">The logger.</param>
/// <param name="validator">The event validator.</param>
/// <seealso cref="CqrsMessageHandler{THandler,TMessage}" />
/// <seealso cref="ICqrsEventHandler{TEvent}" />
public abstract class CqrsEventHandlerBase<TEventHandler, TEvent>(
    ILogger<CqrsEventHandlerBase<TEventHandler, TEvent>> logger, 
    IValidator<TEvent>? validator = null) : CqrsMessageHandler<TEventHandler, TEvent>(logger, validator), ICqrsEventHandler<TEvent>
    where TEvent : class, ICqrsEvent
{
    /// <inheritdoc />
    public async Task<Result> Handle(TEvent @event, CancellationToken cancellationToken = default)
    {
        var guardResult = Result.Try(() => Guard.IsNotNull(@event));

        if (guardResult.IsFailed)
        {
            return guardResult;
        }

        using var activity = ShouldCreateActivity() ? Activity.Current?.Source.StartActivity($"Handle {@event.GetType()}") : null;

        var preHandlingResult = await ExecutePreHandlingAsync(@event, cancellationToken);

        if (!preHandlingResult.IsSuccess)
        {
            return preHandlingResult;
        }

        var validationResult = await ExecuteValidationAsync(@event, cancellationToken);

        if (!validationResult.IsSuccess)
        {
            return validationResult;
        }

        var eventResult = await ExecuteHandleEventAsync(@event, cancellationToken);

        if (!eventResult.IsSuccess)
        {
            return eventResult;
        }

        var postHandlingResult = await ExecutePostHandlingAsync(@event, cancellationToken);

        return !postHandlingResult.IsSuccess
            ? eventResult
            : postHandlingResult;
    }

    private static bool ShouldCreateActivity() => Activity.Current?.Source.HasListeners() ?? false;

    /// <summary>
    ///     Handles the event.
    /// </summary>
    /// <param name="event">The event.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous handle event operation.</returns>
    public abstract Task<Result> HandleEvent(TEvent @event, CancellationToken cancellationToken = default);

    private async Task<Result> ExecutePreHandlingAsync(TEvent @event, CancellationToken cancellationToken)
    {
        using var childSpan = ShouldCreateActivity() ? Activity.Current?.Source.StartActivity("Pre event handling logic") : null;

        childSpan?.AddEvent(new ActivityEvent($"{nameof(CqrsEventHandlerBase<,>)}.{nameof(OnBeforeHandling)}"));

        return await OnBeforeHandling(@event, cancellationToken);
    }

    private async Task<Result> ExecuteValidationAsync(TEvent @event, CancellationToken cancellationToken)
    {
        using var childSpan = ShouldCreateActivity() ? Activity.Current?.Source.StartActivity("Validate event") : null;

        childSpan?.AddEvent(new ActivityEvent($"{nameof(CqrsEventHandlerBase<,>)}.{nameof(ValidateMessage)}"));

        return await ValidateMessage(@event, cancellationToken);
    }

    private async Task<Result> ExecuteHandleEventAsync(TEvent @event, CancellationToken cancellationToken)
    {
        using var childSpan = ShouldCreateActivity() ? Activity.Current?.Source.StartActivity("Handle event") : null;

        childSpan?.AddEvent(new ActivityEvent($"{nameof(CqrsEventHandlerBase<,>)}.{nameof(HandleEvent)}"));

        return await HandleEvent(@event, cancellationToken);
    }

    private async Task<Result> ExecutePostHandlingAsync(TEvent @event, CancellationToken cancellationToken)
    {
        using var childSpan = ShouldCreateActivity() ? Activity.Current?.Source.StartActivity("Post event handling logic") : null;

        childSpan?.AddEvent(new ActivityEvent($"{nameof(CqrsEventHandlerBase<,>)}.{nameof(OnAfterHandling)}"));

        return await OnAfterHandling(@event, cancellationToken);
    }
}
