using Microsoft.Extensions.Logging;
using Twilight.CQRS.Commands;
using Twilight.CQRS.Messaging.Interfaces;
using Twilight.CQRS.Tests.Common;

namespace Twilight.CQRS.Autofac.Tests.Unit.Setup;

internal sealed class TestCqrsCommandHandler(
    IMessageSender messageSender,
    ILogger<TestCqrsCommandHandler> logger,
    IValidator<CqrsCommand<TestParameters>> validator)
    : CqrsCommandHandlerBase<TestCqrsCommandHandler, CqrsCommand<TestParameters>>(messageSender, logger, validator)
{
    public override async Task<Result> HandleCommand(CqrsCommand<TestParameters> command, CancellationToken cancellationToken = default)
        => await Task.FromResult(Result.Ok());
}
