using FluentValidation;
using Microsoft.Extensions.Logging;
using Twilight.CQRS.Queries;

namespace Twilight.CQRS.Tests.Unit.Queries;

public class NonValidatingTestQueryHandler : QueryHandlerBase<NonValidatingTestQueryHandler, Query<string, QueryResponse<string>>, QueryResponse<string>>
{
    public NonValidatingTestQueryHandler(ILogger<NonValidatingTestQueryHandler> logger,
                                            IValidator<Query<string, QueryResponse<string>>>? validator = default)
        : base(logger, validator)
    {
    }

    protected override async Task<QueryResponse<string>> HandleQuery(Query<string, QueryResponse<string>> query, CancellationToken cancellationToken = default)
    {
        var response = new QueryResponse<string>(string.Empty, query.CorrelationId, query.MessageId);

        return await Task.FromResult(response);
    }
}
