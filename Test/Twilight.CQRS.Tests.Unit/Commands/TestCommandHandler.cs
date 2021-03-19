using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Twilight.CQRS.Commands;
using Twilight.CQRS.Events;
using Twilight.CQRS.Messaging.Contracts;
using Twilight.CQRS.Tests.Unit.Shared;

namespace Twilight.CQRS.Tests.Unit.Commands
{
    public sealed class TestCommandHandler : CommandHandlerBase<Command<TestParameters>>
    {
        public TestCommandHandler(IMessageSender messageSender,
                                  IValidator<Command<TestParameters>> validator)
            : base(messageSender, validator)
        {
        }

        protected override async Task HandleCommand(Command<TestParameters> command, CancellationToken cancellationToken = default)
        {
            await MessageSender.Publish(new Event<TestParameters>(command.Params, command.CorrelationId, command.MessageId), cancellationToken);

            await Task.CompletedTask;
        }
    }
}
