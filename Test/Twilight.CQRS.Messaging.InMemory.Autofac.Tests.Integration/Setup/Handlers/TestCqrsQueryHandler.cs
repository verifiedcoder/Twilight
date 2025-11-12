using Microsoft.Extensions.Logging;
using Twilight.CQRS.Queries;
using Twilight.CQRS.Tests.Common;

namespace Twilight.CQRS.Messaging.InMemory.Autofac.Tests.Integration.Setup.Handlers;

[UsedImplicitly]
internal sealed class TestCqrsQueryHandler(
    ITestService service,
    ILogger<TestCqrsQueryHandler> logger,
    IValidator<CqrsQuery<TestParameters, QueryResponse<string>>> validator) : CqrsQueryHandlerBase<TestCqrsQueryHandler, CqrsQuery<TestParameters, QueryResponse<string>>, QueryResponse<string>>(logger, validator)
{
    protected override async Task<Result<QueryResponse<string>>> HandleQuery(CqrsQuery<TestParameters, QueryResponse<string>> query, CancellationToken cancellationToken = default)
    {
        await service.Receive(query.Params.Value);

        var response = new QueryResponse<string>(nameof(TestCqrsQueryHandler), query.CorrelationId, query.MessageId);

        return Result.Ok(response);
    }
}
