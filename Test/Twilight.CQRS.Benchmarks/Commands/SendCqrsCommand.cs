using Twilight.CQRS.Commands;

namespace Twilight.CQRS.Benchmarks.Commands;

internal sealed class SendCqrsCommand : CqrsCommand<MessageParameters>
{
    public SendCqrsCommand(MessageParameters parameters, string correlationId, string? causationId = null)
        : base(parameters, correlationId, causationId)
    {
    }
}
