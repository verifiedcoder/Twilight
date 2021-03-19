using System.Threading;
using System.Threading.Tasks;
using Twilight.CQRS.Commands;
using Twilight.CQRS.Events;
using Twilight.CQRS.Messaging.Contracts;

namespace Twilight.CQRS.Tests.Unit.Commands
{
    public sealed class NonValidatingTestCommandHandler : CommandHandlerBase<Command<string>>
    {
        public NonValidatingTestCommandHandler(IMessageSender messageSender)
            : base(messageSender)
        {
        }

        protected override async Task HandleCommand(Command<string> command, CancellationToken cancellationToken = default)
        {
            await MessageSender.Publish(new Event<string>(command.Params, command.CorrelationId, command.MessageId), cancellationToken);

            await Task.CompletedTask;
        }
    }
}
