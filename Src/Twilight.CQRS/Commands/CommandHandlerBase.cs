using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Twilight.CQRS.Contracts;
using Twilight.CQRS.Messaging.Contracts;

namespace Twilight.CQRS.Commands
{
    /// <summary>
    ///     <para>
    ///         Represents the ability to process (handle) commands. A command handler receives a command and brokers a result.
    ///         A result is either a successful application of the command, or an exception. This class cannot be instantiated.
    ///     </para>
    ///     <para>Implements <see cref="MessageHandler{TCommand}" />.</para>
    ///     <para>Implements <see cref="ICommandHandler{TCommand}" />.</para>
    /// </summary>
    /// <typeparam name="TCommand">The type of the command.</typeparam>
    /// <seealso cref="MessageHandler{TCommand}" />
    /// <seealso cref="ICommandHandler{TCommand}" />
    public abstract class CommandHandlerBase<TCommand> : MessageHandler<TCommand>, ICommandHandler<TCommand>
        where TCommand : class, ICommand
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="CommandHandlerBase{TCommand}" /> class.
        /// </summary>
        /// <param name="messageSender">The message sender.</param>
        /// <param name="validator">The command validator.</param>
        /// <param name="logger">The logger.</param>
        protected CommandHandlerBase(IMessageSender messageSender, ILogger<IMessageHandler<TCommand>> logger, IValidator<TCommand>? validator = default)
            : base(logger, validator) => MessageSender = messageSender;

        /// <summary>
        ///     Gets the message sender.
        /// </summary>
        /// <value>The message sender.</value>
        protected IMessageSender MessageSender { get; }

        /// <inheritdoc />
        public async Task Handle(TCommand command, CancellationToken cancellationToken = default)
        {
            await OnBeforeHandling(command, cancellationToken);

            await ValidateMessage(command, cancellationToken);

            await HandleCommand(command, cancellationToken);

            await OnAfterHandling(command, cancellationToken);
        }

        /// <summary>
        ///     Handles the command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous handle command operation.</returns>
        protected abstract Task HandleCommand(TCommand command, CancellationToken cancellationToken = default);
    }
}
