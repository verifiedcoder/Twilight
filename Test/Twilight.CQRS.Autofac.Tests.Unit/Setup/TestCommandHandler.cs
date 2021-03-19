using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Twilight.CQRS.Commands;
using Twilight.CQRS.Messaging.Contracts;
using Twilight.CQRS.Tests.Unit.Shared;

namespace Twilight.CQRS.Autofac.Tests.Unit.Setup
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
            await Task.CompletedTask;
        }
    }
}
