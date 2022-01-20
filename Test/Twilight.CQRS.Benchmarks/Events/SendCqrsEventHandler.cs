using FluentValidation;
using Microsoft.Extensions.Logging;
using Twilight.CQRS.Events;

namespace Twilight.CQRS.Benchmarks.Events;

internal sealed class SendCqrsEventHandler : CqrsEventHandlerBase<SendCqrsEventHandler, CqrsEvent<MessageParameters>>
{
    public SendCqrsEventHandler(ILogger<SendCqrsEventHandler> logger, IValidator<CqrsEvent<MessageParameters>>? validator = null)
        : base(logger, validator)
    {
    }

    public override async Task HandleEvent(CqrsEvent<MessageParameters> cqrsEvent, CancellationToken cancellationToken = default)
    {
        await Task.CompletedTask;
    }
}
