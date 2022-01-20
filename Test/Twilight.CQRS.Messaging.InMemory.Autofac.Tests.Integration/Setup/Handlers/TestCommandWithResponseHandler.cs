using FluentValidation;
using Microsoft.Extensions.Logging;
using Twilight.CQRS.Commands;
using Twilight.CQRS.Messaging.Interfaces;
using Twilight.CQRS.Tests.Common;

namespace Twilight.CQRS.Messaging.InMemory.Autofac.Tests.Integration.Setup.Handlers;

internal sealed class TestCqrsCommandWithResponseHandler : CqrsCommandHandlerBase<TestCqrsCommandWithResponseHandler, CqrsCommand<TestParameters, CqrsCommandResponse<string>>, CqrsCommandResponse<string>>
{
    private readonly ITestService _service;

    public TestCqrsCommandWithResponseHandler(IMessageSender messageSender,
                                              ITestService service,
                                              ILogger<TestCqrsCommandWithResponseHandler> logger,
                                              IValidator<CqrsCommand<TestParameters, CqrsCommandResponse<string>>> validator)
        : base(messageSender, logger, validator)
        => _service = service;

    protected override async Task<CqrsCommandResponse<string>> HandleCommand(CqrsCommand<TestParameters, CqrsCommandResponse<string>> cqrsCommand, CancellationToken cancellationToken = default)
    {
        await _service.Receive(cqrsCommand.Params.Value);

        var response = new CqrsCommandResponse<string>(nameof(TestCqrsCommandWithResponseHandler), cqrsCommand.CorrelationId, cqrsCommand.MessageId);

        return response;
    }
}
