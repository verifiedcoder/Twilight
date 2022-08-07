using FluentValidation;
using Microsoft.Extensions.Logging;
using Twilight.CQRS.Commands;
using Twilight.CQRS.Events;
using Twilight.CQRS.Messaging.Interfaces;
using Twilight.CQRS.Tests.Common;

namespace Twilight.CQRS.Tests.Unit.Commands;

public sealed class TestCqrsCommandHandler : CqrsCommandHandlerBase<TestCqrsCommandHandler, CqrsCommand<TestParameters>>
{
    public TestCqrsCommandHandler(IMessageSender messageSender,
                                  ILogger<TestCqrsCommandHandler> logger,
                                  IValidator<CqrsCommand<TestParameters>> validator)
        : base(messageSender, logger, validator)
    {
    }

    public override async Task HandleCommand(CqrsCommand<TestParameters> command, CancellationToken cancellationToken = default)
    {
        await MessageSender.Publish(new CqrsEvent<TestParameters>(command.Params, command.CorrelationId, command.MessageId), cancellationToken);

        await Task.CompletedTask;
    }
}
