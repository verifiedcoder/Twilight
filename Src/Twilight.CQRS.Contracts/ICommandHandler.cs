using System.Threading;
using System.Threading.Tasks;

namespace Twilight.CQRS.Contracts
{
    /// <summary>
    ///     Represents a means of handling a command in order to broker a result.
    /// </summary>
    /// <typeparam name="TCommand">The type of the command.</typeparam>
    public interface ICommandHandler<in TCommand> : IMessageHandler<TCommand>
        where TCommand : ICommand
    {
        /// <summary>
        ///     Handles the command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous command handler operation.</returns>
        Task Handle(TCommand command, CancellationToken cancellationToken);
    }
}
