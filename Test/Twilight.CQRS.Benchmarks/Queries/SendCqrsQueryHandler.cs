using FluentValidation;
using Microsoft.Extensions.Logging;
using Twilight.CQRS.Queries;

namespace Twilight.CQRS.Benchmarks.Queries;

internal sealed class SendCqrsQueryHandler : CqrsQueryHandlerBase<SendCqrsQueryHandler, CqrsQuery<MessageParameters, QueryResponse<QueryResponsePayload>>, QueryResponse<QueryResponsePayload>>
{
    public SendCqrsQueryHandler(ILogger<SendCqrsQueryHandler> logger, IValidator<CqrsQuery<MessageParameters, QueryResponse<QueryResponsePayload>>>? validator = null)
        : base(logger, validator)
    {
    }

    protected override async Task<QueryResponse<QueryResponsePayload>> HandleQuery(CqrsQuery<MessageParameters, QueryResponse<QueryResponsePayload>> query, CancellationToken cancellationToken = default)
    {
        var payload = new QueryResponsePayload("CqrsQuery Response");
        var response = new QueryResponse<QueryResponsePayload>(payload, query.CorrelationId, query.MessageId);

        return await Task.FromResult(response);
    }
}
