using Twilight.CQRS.Queries;

namespace Twilight.CQRS.Benchmarks.Queries;

internal sealed class SendCqrsQuery(MessageParameters parameters, string correlationId, string? causationId = null) : CqrsQuery<MessageParameters, QueryResponse<QueryResponsePayload>>(parameters, correlationId, causationId);
