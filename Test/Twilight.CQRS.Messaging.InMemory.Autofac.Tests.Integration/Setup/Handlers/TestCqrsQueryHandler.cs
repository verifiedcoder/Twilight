using FluentValidation;
using Microsoft.Extensions.Logging;
using Twilight.CQRS.Queries;
using Twilight.CQRS.Tests.Common;

namespace Twilight.CQRS.Messaging.InMemory.Autofac.Tests.Integration.Setup.Handlers;

internal sealed class TestCqrsQueryHandler : CqrsQueryHandlerBase<TestCqrsQueryHandler, CqrsQuery<TestParameters, QueryResponse<string>>, QueryResponse<string>>
{
    private readonly ITestService _service;

    public TestCqrsQueryHandler(ITestService service,
                                ILogger<TestCqrsQueryHandler> logger,
                                IValidator<CqrsQuery<TestParameters, QueryResponse<string>>> validator)
        : base(logger, validator)
        => _service = service;

    protected override async Task<QueryResponse<string>> HandleQuery(CqrsQuery<TestParameters, QueryResponse<string>> cqrsQuery, CancellationToken cancellationToken = default)
    {
        await _service.Receive(cqrsQuery.Params.Value);

        var response = new QueryResponse<string>(nameof(TestCqrsQueryHandler), cqrsQuery.CorrelationId, cqrsQuery.MessageId);

        return response;
    }
}
