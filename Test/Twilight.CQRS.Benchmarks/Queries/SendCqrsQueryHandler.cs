using Microsoft.Extensions.Logging;
using Twilight.CQRS.Queries;

namespace Twilight.CQRS.Benchmarks.Queries;

internal sealed class SendCqrsQuery(MessageParameters parameters, string correlationId, string? causationId = null) : CqrsQuery<MessageParameters, QueryResponse<QueryResponsePayload>>(parameters, correlationId, causationId);

internal sealed record QueryResponsePayload(string Response);

[UsedImplicitly]
internal sealed class SendCqrsQueryHandler(
    ILogger<SendCqrsQueryHandler> logger, 
    IValidator<CqrsQuery<MessageParameters, QueryResponse<QueryResponsePayload>>>? validator = null) : CqrsQueryHandlerBase<SendCqrsQueryHandler, CqrsQuery<MessageParameters, QueryResponse<QueryResponsePayload>>, QueryResponse<QueryResponsePayload>>(logger, validator)
{
    protected override async Task<Result<QueryResponse<QueryResponsePayload>>> HandleQuery(CqrsQuery<MessageParameters, QueryResponse<QueryResponsePayload>> query, CancellationToken cancellationToken = default)
    {
        var payload = new QueryResponsePayload("CqrsQuery Response");
        var response = new QueryResponse<QueryResponsePayload>(payload, query.CorrelationId, query.MessageId);

        return await Task.FromResult(Result.Ok(response)).ConfigureAwait(false);
    }
}
