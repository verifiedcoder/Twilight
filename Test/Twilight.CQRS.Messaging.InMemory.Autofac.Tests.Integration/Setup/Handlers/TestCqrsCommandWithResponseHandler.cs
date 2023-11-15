using FluentValidation;
using Microsoft.Extensions.Logging;
using Twilight.CQRS.Commands;
using Twilight.CQRS.Messaging.Interfaces;
using Twilight.CQRS.Tests.Common;

namespace Twilight.CQRS.Messaging.InMemory.Autofac.Tests.Integration.Setup.Handlers;

internal sealed class TestCqrsCommandWithResponseHandler(IMessageSender messageSender,
                                                         ITestService service,
                                                         ILogger<TestCqrsCommandWithResponseHandler> logger,
                                                         IValidator<CqrsCommand<TestParameters, CqrsCommandResponse<string>>> validator)
    : CqrsCommandHandlerBase<TestCqrsCommandWithResponseHandler, CqrsCommand<TestParameters, CqrsCommandResponse<string>>, CqrsCommandResponse<string>>(messageSender, logger, validator)
{
    protected override async Task<CqrsCommandResponse<string>> HandleCommand(CqrsCommand<TestParameters, CqrsCommandResponse<string>> command, CancellationToken cancellationToken = default)
    {
        await service.Receive(command.Params.Value);

        var response = new CqrsCommandResponse<string>(nameof(TestCqrsCommandWithResponseHandler), command.CorrelationId, command.MessageId);

        return response;
    }
}
