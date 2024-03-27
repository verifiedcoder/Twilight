using FluentResults;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Twilight.CQRS.Commands;
using Twilight.CQRS.Messaging.Interfaces;
using Twilight.CQRS.Tests.Common;

namespace Twilight.CQRS.Messaging.InMemory.Autofac.Tests.Integration.Setup.Handlers;

internal sealed class TestCqrsCommandHandler(ITestService service,
                                             IMessageSender messageSender,
                                             ILogger<TestCqrsCommandHandler> logger,
                                             IValidator<CqrsCommand<TestParameters>> validator)
    : CqrsCommandHandlerBase<TestCqrsCommandHandler, CqrsCommand<TestParameters>>(messageSender, logger, validator)
{
    public override async Task<Result> HandleCommand(CqrsCommand<TestParameters> command, CancellationToken cancellationToken = default)
        => await service.Receive(command.Params.Value);
}
