using Microsoft.Extensions.Logging;
using Twilight.CQRS.Benchmarks.Commands;
using Twilight.CQRS.Events;

namespace Twilight.CQRS.Benchmarks.Events;

[UsedImplicitly]
internal sealed class SendCommandReceivedHandler(ILogger<SendCommandReceivedHandler> logger)
    : CqrsEventHandlerBase<SendCommandReceivedHandler, SendCommandReceived>(logger)
{
    public override Task<Result> HandleEvent(SendCommandReceived @event, CancellationToken cancellationToken = default)
        => Task.FromResult(Result.Ok());
}
