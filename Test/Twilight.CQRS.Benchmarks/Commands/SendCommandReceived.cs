using Twilight.CQRS.Events;

namespace Twilight.CQRS.Benchmarks.Commands;

internal sealed class SendCommandReceived : CqrsEvent
{
    public SendCommandReceived(string correlationId, string? causationId = null)
        : base(correlationId, causationId)
    {
    }
}
