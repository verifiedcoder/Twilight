using Microsoft.Extensions.Logging;
using Twilight.CQRS.Events;

namespace Twilight.CQRS.Benchmarks.Events;

internal sealed class SendCqrsEvent(MessageParameters parameters, string correlationId, string? causationId = null)
    : CqrsEvent<MessageParameters>(parameters, correlationId, causationId: causationId);

[UsedImplicitly]
internal sealed class SendCqrsEventHandler(
    ILogger<SendCqrsEventHandler> logger, 
    IValidator<SendCqrsEvent> validator) : CqrsEventHandlerBase<SendCqrsEventHandler, SendCqrsEvent>(logger, validator)
{
    public override async Task<Result> HandleEvent(SendCqrsEvent cqrsEvent, CancellationToken cancellationToken = default)
        => await Task.FromResult(Result.Ok()).ConfigureAwait(false);
}
