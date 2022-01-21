using System.Diagnostics;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Microsoft.Toolkit.Diagnostics;
using Twilight.CQRS.Interfaces;
using Twilight.CQRS.Messaging.Interfaces;

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
/// <seealso cref="CqrsMessageHandler{THandler,TMessage}" />
/// <seealso cref="ICqrsCommandHandler{TCommand}" />
public abstract class CqrsCommandHandlerBase<TCommandHandler, TCommand> : CqrsMessageHandler<TCommandHandler, TCommand>, ICqrsCommandHandler<TCommand>
    where TCommand : class, ICqrsCommand
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="CqrsCommandHandlerBase{TCommandHandler,TCommand}" /> class.
    /// </summary>
    /// <param name="messageSender">The message sender.</param>
    /// <param name="logger">The logger.</param>
    /// <param name="validator">The command validator.</param>
    protected CqrsCommandHandlerBase(IMessageSender messageSender, ILogger<TCommandHandler> logger, IValidator<TCommand>? validator = default)
        : base(logger, validator)
    {
        Guard.IsNotNull(messageSender, nameof(messageSender));

        MessageSender = messageSender;
    }

    /// <summary>
    ///     Gets the message sender.
    /// </summary>
    /// <value>The message sender.</value>
    protected IMessageSender MessageSender { get; }

    /// <inheritdoc />
    public async Task Handle(TCommand command, CancellationToken cancellationToken = default)
    {
        Guard.IsNotNull(command, nameof(command));

        var activitySource = new ActivitySource(ActivitySourceName, AssemblyVersion);

        using var activity = activitySource.StartActivity($"Handle {command.GetType()}");
        {
            using (var childSpan = activitySource.StartActivity(ActivityKind.Consumer))
            {
                childSpan?.AddEvent(new ActivityEvent($"{nameof(CqrsCommandHandlerBase<TCommandHandler, TCommand>)}.{nameof(OnBeforeHandling)}"));

                await OnBeforeHandling(command, cancellationToken);
            }

            using (var childSpan = activitySource.StartActivity(ActivityKind.Consumer))
            {
                childSpan?.AddEvent(new ActivityEvent($"{nameof(CqrsCommandHandlerBase<TCommandHandler, TCommand>)}.{nameof(ValidateMessage)}"));

                await ValidateMessage(command, cancellationToken);
            }

            using (var childSpan = activitySource.StartActivity(ActivityKind.Consumer))
            {
                childSpan?.AddEvent(new ActivityEvent($"{nameof(CqrsCommandHandlerBase<TCommandHandler, TCommand>)}.{nameof(HandleCommand)}"));

                await HandleCommand(command, cancellationToken);
            }

            using (var childSpan = activitySource.StartActivity(ActivityKind.Consumer))
            {
                childSpan?.AddEvent(new ActivityEvent($"{nameof(CqrsCommandHandlerBase<TCommandHandler, TCommand>)}.{nameof(OnAfterHandling)}"));

                await OnAfterHandling(command, cancellationToken);
            }
        }
    }

    /// <summary>
    ///     Handles the command.
    /// </summary>
    /// <param name="command">The command.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous handle command operation.</returns>
    public abstract Task HandleCommand(TCommand command, CancellationToken cancellationToken = default);
}

/// <summary>
///     <para>
///         Represents the ability to process (handle) commands that return a scalar response. A command handler receives a
///         command and directs the cqrsCommand for processing. This class cannot be instantiated.
///     </para>
///     <para>Implements <see cref="CqrsMessageHandler{THandler,TMessage}" />.</para>
///     <para>Implements <see cref="ICqrsCommandHandler{TCommand}" />.</para>
/// </summary>
/// <typeparam name="TCommand">The type of the command.</typeparam>
/// <typeparam name="TResponse">The type of the command response.</typeparam>
/// <typeparam name="TCommandHandler">The type of the command handler.</typeparam>
/// <seealso cref="CqrsMessageHandler{THandler,TMessage}" />
/// <seealso cref="ICqrsCommandHandler{TCommand}" />
public abstract class CqrsCommandHandlerBase<TCommandHandler, TCommand, TResponse> : CqrsMessageHandler<TCommandHandler, TCommand>, ICqrsCommandHandler<TCommand, TResponse>
    where TCommand : class, ICqrsCommand<TResponse>
    where TResponse : class, ICqrsMessage
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="CqrsCommandHandlerBase{TCommandHandler,TCommand}" /> class.
    /// </summary>
    /// <param name="messageSender">The message sender.</param>
    /// <param name="logger">The logger.</param>
    /// <param name="validator">The command validator.</param>
    protected CqrsCommandHandlerBase(IMessageSender messageSender, ILogger<TCommandHandler> logger, IValidator<TCommand>? validator = default)
        : base(logger, validator)
    {
        Guard.IsNotNull(messageSender, nameof(messageSender));

        MessageSender = messageSender;
    }

    /// <summary>
    ///     Gets the message sender.
    /// </summary>
    /// <value>The message sender.</value>
    protected IMessageSender MessageSender { get; }

    /// <inheritdoc />
    public async Task<TResponse> Handle(TCommand command, CancellationToken cancellationToken = default)
    {
        Guard.IsNotNull(command, nameof(command));

        var activitySource = new ActivitySource(ActivitySourceName, AssemblyVersion);

        using var activity = activitySource.StartActivity($"Handle {command.GetType()}");
        {
            using (var childSpan = activitySource.StartActivity(ActivityKind.Consumer))
            {
                childSpan?.AddEvent(new ActivityEvent($"{nameof(CqrsCommandHandlerBase<TCommandHandler, TCommand, TResponse>)}.{nameof(OnBeforeHandling)}"));

                await OnBeforeHandling(command, cancellationToken);
            }

            using (var childSpan = activitySource.StartActivity(ActivityKind.Consumer))
            {
                childSpan?.AddEvent(new ActivityEvent($"{nameof(CqrsCommandHandlerBase<TCommandHandler, TCommand, TResponse>)}.{nameof(ValidateMessage)}"));

                await ValidateMessage(command, cancellationToken);
            }

            TResponse response;

            using (var childSpan = activitySource.StartActivity(ActivityKind.Consumer))
            {
                childSpan?.AddEvent(new ActivityEvent($"{nameof(CqrsCommandHandlerBase<TCommandHandler, TCommand, TResponse>)}.{nameof(HandleCommand)}"));

                response = await HandleCommand(command, cancellationToken);
            }

            using (var childSpan = activitySource.StartActivity(ActivityKind.Consumer))
            {
                childSpan?.AddEvent(new ActivityEvent($"{nameof(CqrsCommandHandlerBase<TCommandHandler, TCommand, TResponse>)}.{nameof(OnAfterHandling)}"));

                await OnAfterHandling(command, cancellationToken);
            }

            return response;
        }
    }

    /// <summary>
    ///     Handles the command.
    /// </summary>
    /// <param name="command">The command.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    ///     <para>A task that represents the asynchronous command handler operation.</para>
    ///     <para>The task result contains the command execution response.</para>
    /// </returns>
    protected abstract Task<TResponse> HandleCommand(TCommand command, CancellationToken cancellationToken = default);
}
