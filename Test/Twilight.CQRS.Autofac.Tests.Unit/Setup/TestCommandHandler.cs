using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Twilight.CQRS.Commands;
using Twilight.CQRS.Messaging.Contracts;
using Twilight.CQRS.Tests.Unit.Shared;

namespace Twilight.CQRS.Autofac.Tests.Unit.Setup
{
    public sealed class TestCommandHandler : CommandHandlerBase<Command<TestParameters>>
    {
        public TestCommandHandler(IMessageSender messageSender,
                                  ILogger<TestCommandHandler> logger,
                                  IValidator<Command<TestParameters>> validator)
            : base(messageSender, logger, validator)
        {
        }

        protected override async Task HandleCommand(Command<TestParameters> command, CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
        }
    }
}
