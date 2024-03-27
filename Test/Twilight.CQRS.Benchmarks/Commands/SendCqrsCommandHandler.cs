using FluentResults;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Twilight.CQRS.Commands;
using Twilight.CQRS.Messaging.Interfaces;

namespace Twilight.CQRS.Benchmarks.Commands;

internal sealed class SendCqrsCommandHandler(
    IMessageSender messageSender, 
    ILogger<SendCqrsCommandHandler> logger, 
    IValidator<CqrsCommand<MessageParameters>>? validator = null) : CqrsCommandHandlerBase<SendCqrsCommandHandler, CqrsCommand<MessageParameters>>(messageSender, logger, validator)
{
    public override async Task<Result> HandleCommand(CqrsCommand<MessageParameters> command, CancellationToken cancellationToken = default)
    {
        var @event = new SendCommandReceived(command.CorrelationId, command.MessageId);

        await MessageSender.Publish(@event, cancellationToken);

        return Result.Ok();
    }
}
