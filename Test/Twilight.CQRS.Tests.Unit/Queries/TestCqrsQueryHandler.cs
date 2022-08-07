using FluentValidation;
using Microsoft.Extensions.Logging;
using Twilight.CQRS.Queries;
using Twilight.CQRS.Tests.Common;

namespace Twilight.CQRS.Tests.Unit.Queries;

public class TestCqrsQueryHandler : CqrsQueryHandlerBase<TestCqrsQueryHandler, CqrsQuery<TestParameters, QueryResponse<TestQueryResponse>>, QueryResponse<TestQueryResponse>>
{
    public TestCqrsQueryHandler(ILogger<TestCqrsQueryHandler> logger,
                                IValidator<CqrsQuery<TestParameters, QueryResponse<TestQueryResponse>>> validator)
        : base(logger, validator)
    {
    }

    protected override async Task<QueryResponse<TestQueryResponse>> HandleQuery(CqrsQuery<TestParameters, QueryResponse<TestQueryResponse>> query, CancellationToken cancellationToken = default)
    {
        var payload = new TestQueryResponse
        {
            Value = "1"
        };

        var response = new QueryResponse<TestQueryResponse>(payload, query.CorrelationId, query.MessageId);

        return await Task.FromResult(response);
    }
}
