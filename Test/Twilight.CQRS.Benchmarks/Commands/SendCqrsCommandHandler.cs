using FluentValidation;
using Microsoft.Extensions.Logging;
using Twilight.CQRS.Commands;
using Twilight.CQRS.Messaging.Interfaces;

namespace Twilight.CQRS.Benchmarks.Commands;

internal sealed class SendCqrsCommandHandler : CqrsCommandHandlerBase<SendCqrsCommandHandler, CqrsCommand<MessageParameters>>
{
    public SendCqrsCommandHandler(IMessageSender messageSender, ILogger<SendCqrsCommandHandler> logger, IValidator<CqrsCommand<MessageParameters>>? validator = null)
        : base(messageSender, logger, validator)
    {
    }

    public override async Task HandleCommand(CqrsCommand<MessageParameters> cqrsCommand, CancellationToken cancellationToken = default)
    {
        var @event = new SendCommandReceived(cqrsCommand.CorrelationId, cqrsCommand.MessageId);

        await MessageSender.Publish(@event, cancellationToken);
    }
}
