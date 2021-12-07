using FluentValidation;
using Microsoft.Extensions.Logging;
using Twilight.CQRS.Commands;
using Twilight.CQRS.Messaging.Interfaces;
using Twilight.CQRS.Tests.Unit.Common;

namespace Twilight.CQRS.Messaging.InMemory.Autofac.Tests.Integration.Setup.Handlers;

public sealed class TestCommandWithResponseHandler : CommandHandlerBase<TestCommandWithResponseHandler, Command<TestParameters, CommandResponse<string>>, CommandResponse<string>>
{
    private readonly ITestService _service;

    public TestCommandWithResponseHandler(IMessageSender messageSender,
                                            ITestService service,
                                            ILogger<TestCommandWithResponseHandler> logger,
                                            IValidator<Command<TestParameters, CommandResponse<string>>> validator)
        : base(messageSender, logger, validator)
        => _service = service;

    protected override async Task<CommandResponse<string>> HandleCommand(Command<TestParameters, CommandResponse<string>> command, CancellationToken cancellationToken = default)
    {
        await _service.Receive(command.Params.Value);

        var response = new CommandResponse<string>(nameof(TestCommandWithResponseHandler), command.CorrelationId, command.MessageId);

        return response;
    }
}
