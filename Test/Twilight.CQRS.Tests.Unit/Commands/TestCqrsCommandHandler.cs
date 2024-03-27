using FluentResults;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Twilight.CQRS.Commands;
using Twilight.CQRS.Events;
using Twilight.CQRS.Messaging.Interfaces;
using Twilight.CQRS.Tests.Common;

namespace Twilight.CQRS.Tests.Unit.Commands;

public sealed class TestCqrsCommandHandler(
    IMessageSender messageSender,
    ILogger<TestCqrsCommandHandler> logger,
    IValidator<CqrsCommand<TestParameters>> validator)
    : CqrsCommandHandlerBase<TestCqrsCommandHandler, CqrsCommand<TestParameters>>(messageSender, logger, validator)
{
    public override async Task<Result> HandleCommand(CqrsCommand<TestParameters> command, CancellationToken cancellationToken = default)
    {
        await MessageSender.Publish(new CqrsEvent<TestParameters>(command.Params, command.CorrelationId, command.MessageId), cancellationToken);

        return await Task.FromResult(Result.Ok());
    }
}
