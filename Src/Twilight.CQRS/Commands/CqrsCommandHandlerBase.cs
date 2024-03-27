using System.Diagnostics;
using Microsoft.Extensions.Logging;
using CommunityToolkit.Diagnostics;
using FluentResults;
using FluentValidation;
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
        Guard.IsNotNull(messageSender);

        MessageSender = messageSender;
    }

    /// <summary>
    ///     Gets the message sender.
    /// </summary>
    /// <value>The message sender.</value>
    protected IMessageSender MessageSender { get; }

    /// <inheritdoc />
    public async Task<Result> Handle(TCommand command, CancellationToken cancellationToken = default)
    {
        var guardResult = Result.Try(() =>
        {
            Guard.IsNotNull(command);
        });

        if (guardResult.IsFailed)
        {
            return guardResult;
        }
        
        using var activity = Activity.Current?.Source.StartActivity($"Handle {command.GetType()}");
        {
            using (var childSpan = Activity.Current?.Source.StartActivity("Pre command handling actions"))
            {
                childSpan?.AddEvent(new ActivityEvent($"{nameof(CqrsCommandHandlerBase<TCommandHandler, TCommand>)}.{nameof(OnBeforeHandling)}"));

                var result = await OnBeforeHandling(command, cancellationToken);

                if (!result.IsSuccess)
                {
                    return result;
                }
            }

            using (var childSpan = Activity.Current?.Source.StartActivity("Validate command"))
            {
                childSpan?.AddEvent(new ActivityEvent($"{nameof(CqrsCommandHandlerBase<TCommandHandler, TCommand>)}.{nameof(ValidateMessage)}"));

                var result = await ValidateMessage(command, cancellationToken);

                if (!result.IsSuccess)
                {
                    return result;
                }
            }

            using (var childSpan = Activity.Current?.Source.StartActivity("Handle command"))
            {
                childSpan?.AddEvent(new ActivityEvent($"{nameof(CqrsCommandHandlerBase<TCommandHandler, TCommand>)}.{nameof(HandleCommand)}"));

                var result = await HandleCommand(command, cancellationToken);

                if (!result.IsSuccess)
                {
                    return result;
                }
            }

            using (var childSpan = Activity.Current?.Source.StartActivity("Post command handling actions"))
            {
                childSpan?.AddEvent(new ActivityEvent($"{nameof(CqrsCommandHandlerBase<TCommandHandler, TCommand>)}.{nameof(OnAfterHandling)}"));

                var result = await OnAfterHandling(command, cancellationToken);

                if (!result.IsSuccess)
                {
                    return result;
                }
            }
        }

        return Result.Ok();
    }

    /// <summary>
    ///     Handles the command.
    /// </summary>
    /// <param name="command">The command.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous handle command operation.</returns>
    public abstract Task<Result> HandleCommand(TCommand command, CancellationToken cancellationToken = default);
}

/// <summary>
///     <para>
///         Represents the ability to process (handle) commands that return a scalar response. A command handler receives a
///         command and directs the command for processing. This class cannot be instantiated.
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
    where TResponse : class,ICqrsMessage
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="CqrsCommandHandlerBase{TCommandHandler,TCommand}" /> class.
    /// </summary>
    /// <param name="messageSender">The message sender.</param>
    /// <param name="logger">The logger.</param>
    /// <param name="validator">The command validator.</param>
    protected CqrsCommandHandlerBase(IMessageSender messageSender, ILogger<TCommandHandler> logger, IValidator<TCommand>? validator = default)
        : base(logger, validator)
        => MessageSender = messageSender;

    /// <summary>
    ///     Gets the message sender.
    /// </summary>
    /// <value>The message sender.</value>
    protected IMessageSender MessageSender { get; }

    /// <inheritdoc />
    public async Task<Result<TResponse>> Handle(TCommand command, CancellationToken cancellationToken = default)
    {
        var guardResult = Result.Try(() =>
        {
            Guard.IsNotNull(command);
        });

        if (guardResult.IsFailed)
        {
            return guardResult;
        }

        Result<TResponse> commandResult;

        using var activity = Activity.Current?.Source.StartActivity($"Handle {command.GetType()}");
        {
            using (var childSpan = Activity.Current?.Source.StartActivity("pre command handling actions"))
            {
                childSpan?.AddEvent(new ActivityEvent($"{nameof(CqrsCommandHandlerBase<TCommandHandler, TCommand, TResponse>)}.{nameof(OnBeforeHandling)}"));

                var result = await OnBeforeHandling(command, cancellationToken);

                if (!result.IsSuccess)
                {
                    return result;
                }
            }

            using (var childSpan = Activity.Current?.Source.StartActivity("Validate command"))
            {
                childSpan?.AddEvent(new ActivityEvent($"{nameof(CqrsCommandHandlerBase<TCommandHandler, TCommand, TResponse>)}.{nameof(ValidateMessage)}"));

                var result = await ValidateMessage(command, cancellationToken);

                if (!result.IsSuccess)
                {
                    return result;
                }
            }

            using (var childSpan = Activity.Current?.Source.StartActivity("Handle command"))
            {
                childSpan?.AddEvent(new ActivityEvent($"{nameof(CqrsCommandHandlerBase<TCommandHandler, TCommand, TResponse>)}.{nameof(HandleCommand)}"));

                var result = await HandleCommand(command, cancellationToken);

                if (!result.IsSuccess)
                {
                    return result;
                }

                commandResult = result;
            }

            using (var childSpan = Activity.Current?.Source.StartActivity("Post command handling actions"))
            {
                childSpan?.AddEvent(new ActivityEvent($"{nameof(CqrsCommandHandlerBase<TCommandHandler, TCommand, TResponse>)}.{nameof(OnAfterHandling)}"));

                var result = await OnAfterHandling(command, cancellationToken);

                if (!result.IsSuccess)
                {
                    return result;
                }
            }
        }

        return commandResult;
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
    protected abstract Task<Result<TResponse>> HandleCommand(TCommand command, CancellationToken cancellationToken = default);
}
