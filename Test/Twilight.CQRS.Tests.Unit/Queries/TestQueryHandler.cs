using FluentValidation;
using Microsoft.Extensions.Logging;
using Twilight.CQRS.Queries;
using Twilight.CQRS.Tests.Unit.Common;

namespace Twilight.CQRS.Tests.Unit.Queries;

public class TestQueryHandler : QueryHandlerBase<TestQueryHandler, Query<TestParameters, QueryResponse<TestQueryResponse>>, QueryResponse<TestQueryResponse>>
{
    public TestQueryHandler(ILogger<TestQueryHandler> logger,
                            IValidator<Query<TestParameters, QueryResponse<TestQueryResponse>>> validator)
        : base(logger, validator)
    {
    }

    protected override async Task<QueryResponse<TestQueryResponse>> HandleQuery(Query<TestParameters, QueryResponse<TestQueryResponse>> query, CancellationToken cancellationToken = default)
    {
        var payload = new TestQueryResponse
        {
            Value = "1"
        };

        var response = new QueryResponse<TestQueryResponse>(payload, query.CorrelationId, query.MessageId);

        return await Task.FromResult(response);
    }
}
