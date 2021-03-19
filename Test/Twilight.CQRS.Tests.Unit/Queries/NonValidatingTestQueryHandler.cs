using System.Threading;
using System.Threading.Tasks;
using Twilight.CQRS.Queries;

namespace Twilight.CQRS.Tests.Unit.Queries
{
    public class NonValidatingTestQueryHandler : QueryHandlerBase<Query<string, QueryResponse<string>>, QueryResponse<string>>
    {
        protected override async Task<QueryResponse<string>> HandleQuery(Query<string, QueryResponse<string>> query, CancellationToken cancellationToken = default)
        {
            var response = new QueryResponse<string>(string.Empty, query.CorrelationId, query.MessageId);

            return await Task.FromResult(response);
        }
    }
}
