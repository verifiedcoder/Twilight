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
///     <para>Implements <see cref="MessageHandler{THandler, TCommand}" />.</para>
///     <para>Implements <see cref="ICommandHandler{TCommand}" />.</para>
/// </summary>
/// <typeparam name="TCommand">The type of the command.</typeparam>
/// <typeparam name="TCommandHandler">The type of the command handler.</typeparam>
/// <seealso cref="MessageHandler{THandler, TCommand}" />
/// <seealso cref="ICommandHandler{TCommand}" />
public abstract class CommandHandlerBase<TCommandHandler, TCommand> : MessageHandler<TCommandHandler, TCommand>, ICommandHandler<TCommand>
    where TCommand : class, ICommand
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="CommandHandlerBase{TCommandHandler, TCommand}" /> class.
    /// </summary>
    /// <param name="messageSender">The message sender.</param>
    /// <param name="logger">The logger.</param>
    /// <param name="validator">The command validator.</param>
    protected CommandHandlerBase(IMessageSender messageSender, ILogger<TCommandHandler> logger, IValidator<TCommand>? validator = default)
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
            using (var childSpan = activitySource.StartActivity("OnBeforeHandlingCommand", ActivityKind.Consumer))
            {
                childSpan?.AddEvent(new ActivityEvent($"{nameof(CommandHandlerBase<TCommandHandler, TCommand>)}.{nameof(OnBeforeHandling)}"));

                await OnBeforeHandling(command, cancellationToken);
            }

            using (var childSpan = activitySource.StartActivity("ValidateCommand", ActivityKind.Consumer))
            {
                childSpan?.AddEvent(new ActivityEvent($"{nameof(CommandHandlerBase<TCommandHandler, TCommand>)}.{nameof(ValidateMessage)}"));

                await ValidateMessage(command, cancellationToken);
            }

            using (var childSpan = activitySource.StartActivity("HandleCommand", ActivityKind.Consumer))
            {
                childSpan?.AddEvent(new ActivityEvent($"{nameof(CommandHandlerBase<TCommandHandler, TCommand>)}.{nameof(HandleCommand)}"));

                await HandleCommand(command, cancellationToken);
            }

            using (var childSpan = activitySource.StartActivity("OnAfterHandlingCommand", ActivityKind.Consumer))
            {
                childSpan?.AddEvent(new ActivityEvent($"{nameof(CommandHandlerBase<TCommandHandler, TCommand>)}.{nameof(OnAfterHandling)}"));

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
///         command and directs the command for processing. This class cannot be instantiated.
///     </para>
///     <para>Implements <see cref="MessageHandler{THandler, TCommand}" />.</para>
///     <para>Implements <see cref="ICommandHandler{TCommand,TResponse}" />.</para>
/// </summary>
/// <typeparam name="TCommand">The type of the command.</typeparam>
/// <typeparam name="TResponse">The type of the command response.</typeparam>
/// <typeparam name="TCommandHandler">The type of the command handler.</typeparam>
/// <seealso cref="MessageHandler{THandler, TCommand}" />
/// <seealso cref="ICommandHandler{TCommand, TResponse}" />
public abstract class CommandHandlerBase<TCommandHandler, TCommand, TResponse> : MessageHandler<TCommandHandler, TCommand>, ICommandHandler<TCommand, TResponse>
    where TCommand : class, ICommand<TResponse>
    where TResponse : class, IMessage
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="CommandHandlerBase{TCommand,TResponse}" /> class.
    /// </summary>
    /// <param name="messageSender">The message sender.</param>
    /// <param name="logger">The logger.</param>
    /// <param name="validator">The command validator.</param>
    protected CommandHandlerBase(IMessageSender messageSender, ILogger<TCommandHandler> logger, IValidator<TCommand>? validator = default)
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
            using (var childSpan = activitySource.StartActivity("OnBeforeHandlingCommand", ActivityKind.Consumer))
            {
                childSpan?.AddEvent(new ActivityEvent($"{nameof(CommandHandlerBase<TCommandHandler, TCommand, TResponse>)}.{nameof(OnBeforeHandling)}"));

                await OnBeforeHandling(command, cancellationToken);
            }

            using (var childSpan = activitySource.StartActivity("ValidateCommand", ActivityKind.Consumer))
            {
                childSpan?.AddEvent(new ActivityEvent($"{nameof(CommandHandlerBase<TCommandHandler, TCommand, TResponse>)}.{nameof(ValidateMessage)}"));

                await ValidateMessage(command, cancellationToken);
            }

            TResponse response;

            using (var childSpan = activitySource.StartActivity("HandleCommand", ActivityKind.Consumer))
            {
                childSpan?.AddEvent(new ActivityEvent($"{nameof(CommandHandlerBase<TCommandHandler, TCommand, TResponse>)}.{nameof(HandleCommand)}"));

                response = await HandleCommand(command, cancellationToken);
            }

            using (var childSpan = activitySource.StartActivity("OnAfterHandlingCommand", ActivityKind.Consumer))
            {
                childSpan?.AddEvent(new ActivityEvent($"{nameof(CommandHandlerBase<TCommandHandler, TCommand, TResponse>)}.{nameof(OnAfterHandling)}"));

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

