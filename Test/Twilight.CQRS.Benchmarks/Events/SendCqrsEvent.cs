using Twilight.CQRS.Events;

namespace Twilight.CQRS.Benchmarks.Events;

internal sealed class SendCqrsEvent : CqrsEvent<MessageParameters>
{
    public SendCqrsEvent(MessageParameters parameters, string correlationId, string? causationId = null)
        : base(parameters, correlationId, causationId)
    {
    }
}
