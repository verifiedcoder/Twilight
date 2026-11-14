using Microsoft.Extensions.Logging;
using Twilight.CQRS.Queries;

namespace Twilight.CQRS.Benchmarks.Queries;

internal sealed class SendCqrsQuery(MessageParameters parameters, string correlationId, string? causationId = null) : CqrsQuery<MessageParameters, QueryResponse<QueryResponsePayload>>(parameters, correlationId, causationId: causationId);

internal sealed record QueryResponsePayload(string Response);

[UsedImplicitly]
internal sealed class SendCqrsQueryHandler(
    ILogger<SendCqrsQueryHandler> logger, 
    IValidator<SendCqrsQuery> validator) : CqrsQueryHandlerBase<SendCqrsQueryHandler, SendCqrsQuery, QueryResponse<QueryResponsePayload>>(logger, validator)
{
    protected override async Task<Result<QueryResponse<QueryResponsePayload>>> HandleQuery(SendCqrsQuery query, CancellationToken cancellationToken = default)
    {
        var payload = new QueryResponsePayload("CqrsQuery Response");
        var response = new QueryResponse<QueryResponsePayload>(payload, query.CorrelationId, causationId: query.MessageId);

        return await Task.FromResult(Result.Ok(response)).ConfigureAwait(false);
    }
}
