using Microsoft.Extensions.Logging;
using Twilight.CQRS.Queries;

namespace Twilight.CQRS.Tests.Unit.Queries;

[UsedImplicitly]
internal sealed class NonValidatingTestCqrsQueryHandler(
    ILogger<NonValidatingTestCqrsQueryHandler> logger,
    IValidator<CqrsQuery<string, QueryResponse<string>>>? validator = null) : CqrsQueryHandlerBase<NonValidatingTestCqrsQueryHandler, CqrsQuery<string, QueryResponse<string>>, QueryResponse<string>>(logger, validator)
{
    protected override async Task<Result<QueryResponse<string>>> HandleQuery(CqrsQuery<string, QueryResponse<string>> query, CancellationToken cancellationToken = default)
    {
        var response = new QueryResponse<string>(string.Empty, query.CorrelationId, query.MessageId);

        return await Task.FromResult(Result.Ok(response));
    }
}
