using CommunityToolkit.Diagnostics;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using Twilight.CQRS.Interfaces;
using Twilight.CQRS.Messaging.Interfaces;
// ReSharper disable ExplicitCallerInfoArgument as false positive for StartActivity

namespace Twilight.CQRS.Commands;

/// <summary>
///     <para>
///         Represents the ability to process (handle) commands. A command handler receives a command and brokers a result.
///         A result is either a successful application of the command, or an exception. This class cannot be instantiated.
///     </para>
///     <para>Implements <see cref="CqrsMessageHandler{THandler,TMessage}" />.</para>
///     <para>Implements <see cref="ICqrsCommandHandler{TCommand}" />.</para>
/// </summary>
/// <typeparam name="TCommand">The type of the command.</typeparam>
/// <typeparam name="TCommandHandler">The type of the command handler.</typeparam>
/// <param name="messageSender">The message sender.</param>
/// <param name="logger">The logger.</param>
/// <param name="validator">The command validator.</param>
/// <seealso cref="CqrsMessageHandler{THandler,TMessage}" />
/// <seealso cref="ICqrsCommandHandler{TCommand}" />
public abstract class CqrsCommandHandlerBase<TCommandHandler, TCommand>(
    IMessageSender messageSender, 
    ILogger<CqrsCommandHandlerBase<TCommandHandler, TCommand>> logger, 
    IValidator<TCommand>? validator = null) : CqrsMessageHandler<TCommandHandler, TCommand>(logger, validator), ICqrsCommandHandler<TCommand>
    where TCommand : class, ICqrsCommand
{
    /// <summary>
    ///     Gets the message sender.
    /// </summary>
    /// <value>The message sender.</value>
    protected IMessageSender MessageSender { get; } = messageSender;

    /// <inheritdoc />
    public async Task<Result> Handle(TCommand command, CancellationToken cancellationToken = default)
    {
        var guardResult = Result.Try(() => Guard.IsNotNull(command));
        if (guardResult.IsFailed)
        {
            return guardResult;
        }

        using var activity = ShouldCreateActivity() ? Activity.Current?.Source.StartActivity($"Handle {command.GetType()}") : null;

        var preHandlingResult = await ExecutePreHandlingAsync(command, cancellationToken);

        if (!preHandlingResult.IsSuccess)
        {
            return preHandlingResult;
        }

        var validationResult = await ExecuteValidationAsync(command, cancellationToken);

        if (!validationResult.IsSuccess)
        {
            return validationResult;
        }

        var handleResult = await ExecuteHandleCommandAsync(command, cancellationToken);

        if (!handleResult.IsSuccess)
        {
            return handleResult;
        }

        var postHandlingResult = await ExecutePostHandlingAsync(command, cancellationToken);

        return postHandlingResult.IsSuccess
            ? Result.Ok()
            : postHandlingResult;
    }

    private static bool ShouldCreateActivity() => Activity.Current?.Source.HasListeners() ?? false;

    /// <summary>
    ///     Handles the command.
    /// </summary>
    /// <param name="command">The command.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous handle command operation.</returns>
    public abstract Task<Result> HandleCommand(TCommand command, CancellationToken cancellationToken = default);

    private async Task<Result> ExecutePreHandlingAsync(TCommand command, CancellationToken cancellationToken)
    {
        using var childSpan = ShouldCreateActivity() ? Activity.Current?.Source.StartActivity("Pre command handling actions") : null;

        childSpan?.AddEvent(new ActivityEvent($"{nameof(CqrsCommandHandlerBase<,>)}.{nameof(OnBeforeHandling)}"));

        return await OnBeforeHandling(command, cancellationToken);
    }

    private async Task<Result> ExecuteValidationAsync(TCommand command, CancellationToken cancellationToken)
    {
        using var childSpan = ShouldCreateActivity() ? Activity.Current?.Source.StartActivity("Validate command") : null;

        childSpan?.AddEvent(new ActivityEvent($"{nameof(CqrsCommandHandlerBase<,>)}.{nameof(ValidateMessage)}"));

        return await ValidateMessage(command, cancellationToken);
    }

    private async Task<Result> ExecuteHandleCommandAsync(TCommand command, CancellationToken cancellationToken)
    {
        using var childSpan = ShouldCreateActivity() ? Activity.Current?.Source.StartActivity("Handle command") : null;

        childSpan?.AddEvent(new ActivityEvent($"{nameof(CqrsCommandHandlerBase<,>)}.{nameof(HandleCommand)}"));

        return await HandleCommand(command, cancellationToken);
    }

    private async Task<Result> ExecutePostHandlingAsync(TCommand command, CancellationToken cancellationToken)
    {
        using var childSpan = ShouldCreateActivity() ? Activity.Current?.Source.StartActivity("Post command handling actions") : null;

        childSpan?.AddEvent(new ActivityEvent($"{nameof(CqrsCommandHandlerBase<,>)}.{nameof(OnAfterHandling)}"));

        return await OnAfterHandling(command, cancellationToken);
    }
}

/// <summary>
///     <para>
///         Represents the ability to process (handle) commands that return a scalar response. A command handler receives a
///         command and directs the command for processing. This class cannot be instantiated.
///     </para>
///     <para>Implements <see cref="CqrsMessageHandler{THandler,TMessage}" />.</para>
///     <para>Implements <see cref="ICqrsCommandHandler{TCommand,TResponse}" />.</para>
/// </summary>
/// <typeparam name="TCommand">The type of the command.</typeparam>
/// <typeparam name="TResponse">The type of the command response.</typeparam>
/// <typeparam name="TCommandHandler">The type of the command handler.</typeparam>
/// <param name="messageSender">The message sender.</param>
/// <param name="logger">The logger.</param>
/// <param name="validator">The command validator.</param>
/// <seealso cref="CqrsMessageHandler{THandler,TMessage}" />
/// <seealso cref="ICqrsCommandHandler{TCommand,TResponse}" />
public abstract class CqrsCommandHandlerBase<TCommandHandler, TCommand, TResponse>(
    IMessageSender messageSender, 
    ILogger<CqrsCommandHandlerBase<TCommandHandler, TCommand, TResponse>> logger, 
    IValidator<TCommand>? validator = null) : CqrsMessageHandler<TCommandHandler, TCommand>(logger, validator), ICqrsCommandHandler<TCommand, TResponse>
    where TCommand : class, ICqrsCommand<TResponse>
    where TResponse : class, ICqrsMessage
{
    /// <summary>
    ///     Gets the message sender.
    /// </summary>
    /// <value>The message sender.</value>
    protected IMessageSender MessageSender { get; } = messageSender;

    /// <inheritdoc />
    public async Task<Result<TResponse>> Handle(TCommand command, CancellationToken cancellationToken = default)
    {
        var guardResult = Result.Try(() => Guard.IsNotNull(command));

        if (guardResult.IsFailed)
        {
            return guardResult;
        }

        using var activity = ShouldCreateActivity() ? Activity.Current?.Source.StartActivity($"Handle {command.GetType()}") : null;

        var preHandlingResult = await ExecutePreHandlingAsync(command, cancellationToken);

        if (!preHandlingResult.IsSuccess)
        {
            return preHandlingResult;
        }

        var validationResult = await ExecuteValidationAsync(command, cancellationToken);

        if (!validationResult.IsSuccess)
        {
            return validationResult;
        }

        var commandResult = await ExecuteHandleCommandAsync(command, cancellationToken);

        if (!commandResult.IsSuccess)
        {
            return commandResult;
        }

        var postHandlingResult = await ExecutePostHandlingAsync(command, cancellationToken);

        return postHandlingResult.IsSuccess
            ? commandResult
            : postHandlingResult;
    }

    private static bool ShouldCreateActivity() => Activity.Current?.Source.HasListeners() ?? false;

    /// <summary>
    ///     Handles the command.
    /// </summary>
    /// <param name="command">The command.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    ///     <para>A task that represents the asynchronous command handler operation.</para>
    ///     <para>The task result contains the command execution response.</para>
    /// </returns>
    protected abstract Task<Result<TResponse>> HandleCommand(TCommand command, CancellationToken cancellationToken = default);

    private async Task<Result<TResponse>> ExecutePreHandlingAsync(TCommand command, CancellationToken cancellationToken)
    {
        using var childSpan = ShouldCreateActivity() ? Activity.Current?.Source.StartActivity("Pre command handling actions") : null;

        childSpan?.AddEvent(new ActivityEvent($"{nameof(CqrsCommandHandlerBase<,,>)}.{nameof(OnBeforeHandling)}"));

        return await OnBeforeHandling(command, cancellationToken);
    }

    private async Task<Result<TResponse>> ExecuteValidationAsync(TCommand command, CancellationToken cancellationToken)
    {
        using var childSpan = ShouldCreateActivity() ? Activity.Current?.Source.StartActivity("Validate command") : null;

        childSpan?.AddEvent(new ActivityEvent($"{nameof(CqrsCommandHandlerBase<,,>)}.{nameof(ValidateMessage)}"));

        return await ValidateMessage(command, cancellationToken);
    }

    private async Task<Result<TResponse>> ExecuteHandleCommandAsync(TCommand command, CancellationToken cancellationToken)
    {
        using var childSpan = ShouldCreateActivity() ? Activity.Current?.Source.StartActivity("Handle command") : null;

        childSpan?.AddEvent(new ActivityEvent($"{nameof(CqrsCommandHandlerBase<,,>)}.{nameof(HandleCommand)}"));

        return await HandleCommand(command, cancellationToken);
    }

    private async Task<Result<TResponse>> ExecutePostHandlingAsync(TCommand command, CancellationToken cancellationToken)
    {
        using var childSpan = ShouldCreateActivity() ? Activity.Current?.Source.StartActivity("Post command handling actions") : null;

        childSpan?.AddEvent(new ActivityEvent($"{nameof(CqrsCommandHandlerBase<,,>)}.{nameof(OnAfterHandling)}"));

        return await OnAfterHandling(command, cancellationToken);
    }
}