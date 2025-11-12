using Microsoft.Extensions.Logging;
using Twilight.CQRS.Commands;
using Twilight.CQRS.Events;
using Twilight.CQRS.Messaging.Interfaces;

namespace Twilight.CQRS.Benchmarks.Commands;

internal sealed class SendCqrsCommand(MessageParameters parameters, string correlationId, string? causationId = null) : CqrsCommand<MessageParameters>(parameters, correlationId, causationId);

internal sealed class SendCommandReceived(string correlationId, string? causationId = null) : CqrsEvent(correlationId, causationId);

[UsedImplicitly]
internal sealed class SendCqrsCommandHandler(
    IMessageSender messageSender, 
    ILogger<SendCqrsCommandHandler> logger, 
    IValidator<CqrsCommand<MessageParameters>>? validator = null) : CqrsCommandHandlerBase<SendCqrsCommandHandler, CqrsCommand<MessageParameters>>(messageSender, logger, validator)
{
    public override async Task<Result> HandleCommand(CqrsCommand<MessageParameters> command, CancellationToken cancellationToken = default)
    {
        var @event = new SendCommandReceived(command.CorrelationId, command.MessageId);

        await MessageSender.Publish(@event, cancellationToken).ConfigureAwait(false);

        return Result.Ok();
    }
}
