using FluentValidation;
using Microsoft.Extensions.Logging;
using Twilight.CQRS.Queries;
using Twilight.CQRS.Tests.Unit.Common;

namespace Twilight.CQRS.Messaging.InMemory.Autofac.Tests.Integration.Setup.Handlers;

public sealed class TestQueryHandler : QueryHandlerBase<TestQueryHandler, Query<TestParameters, QueryResponse<string>>, QueryResponse<string>>
{
    private readonly ITestService _service;

    public TestQueryHandler(ITestService service,
                            ILogger<TestQueryHandler> logger,
                            IValidator<Query<TestParameters, QueryResponse<string>>> validator)
        : base(logger, validator)
        => _service = service;

    protected override async Task<QueryResponse<string>> HandleQuery(Query<TestParameters, QueryResponse<string>> query, CancellationToken cancellationToken = default)
    {
        await _service.Receive(query.Params.Value);

        var response = new QueryResponse<string>(nameof(TestQueryHandler), query.CorrelationId, query.MessageId);

        return response;
    }
}
