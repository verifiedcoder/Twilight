using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Twilight.CQRS.Commands;
using Twilight.CQRS.Messaging.Contracts;
using Twilight.CQRS.Tests.Unit.Shared;

namespace Twilight.CQRS.Messaging.InMemory.Autofac.Tests.Integration.Setup.Handlers
{
    public sealed class TestCommandHandler : CommandHandlerBase<Command<TestParameters>>
    {
        private readonly ITestService _service;

        public TestCommandHandler(ITestService service,
                                  IMessageSender messageSender,
                                  ILogger<TestCommandHandler> logger,
                                  IValidator<Command<TestParameters>> validator)
            : base(messageSender, logger, validator) => _service = service;

        protected override async Task HandleCommand(Command<TestParameters> command, CancellationToken cancellationToken = default)
        {
            await _service.Receive(command.Params.Value);
        }
    }
}
