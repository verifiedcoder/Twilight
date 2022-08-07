using FluentValidation;
using Microsoft.Extensions.Logging;
using Twilight.CQRS.Queries;

namespace Twilight.CQRS.Tests.Unit.Queries;

internal class NonValidatingTestCqrsQueryHandler : CqrsQueryHandlerBase<NonValidatingTestCqrsQueryHandler, CqrsQuery<string, QueryResponse<string>>, QueryResponse<string>>
{
    public NonValidatingTestCqrsQueryHandler(ILogger<NonValidatingTestCqrsQueryHandler> logger,
                                             IValidator<CqrsQuery<string, QueryResponse<string>>>? validator = default)
        : base(logger, validator)
    {
    }

    protected override async Task<QueryResponse<string>> HandleQuery(CqrsQuery<string, QueryResponse<string>> query, CancellationToken cancellationToken = default)
    {
        var response = new QueryResponse<string>(string.Empty, query.CorrelationId, query.MessageId);

        return await Task.FromResult(response);
    }
}
