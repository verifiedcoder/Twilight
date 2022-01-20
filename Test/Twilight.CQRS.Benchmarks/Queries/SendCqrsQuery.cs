using Twilight.CQRS.Queries;

namespace Twilight.CQRS.Benchmarks.Queries;

internal sealed class SendCqrsQuery : CqrsQuery<MessageParameters, QueryResponse<QueryResponsePayload>>
{
    public SendCqrsQuery(MessageParameters parameters, string correlationId, string? causationId = null)
        : base(parameters, correlationId, causationId)
    {
    }
}
