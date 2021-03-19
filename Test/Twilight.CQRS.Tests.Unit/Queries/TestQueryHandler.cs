using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Twilight.CQRS.Queries;
using Twilight.CQRS.Tests.Unit.Shared;

namespace Twilight.CQRS.Tests.Unit.Queries
{
    public class TestQueryHandler : QueryHandlerBase<Query<TestParameters, QueryResponse<TestQueryResponse>>, QueryResponse<TestQueryResponse>>
    {
        public TestQueryHandler(IValidator<Query<TestParameters, QueryResponse<TestQueryResponse>>> validator)
            : base(validator)
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
}
