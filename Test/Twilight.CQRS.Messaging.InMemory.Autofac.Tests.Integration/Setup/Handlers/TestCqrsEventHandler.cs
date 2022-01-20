using FluentValidation;
using Microsoft.Extensions.Logging;
using Twilight.CQRS.Events;
using Twilight.CQRS.Tests.Common;

namespace Twilight.CQRS.Messaging.InMemory.Autofac.Tests.Integration.Setup.Handlers;

internal sealed class TestCqrsEventHandler : CqrsEventHandlerBase<TestCqrsEventHandler, CqrsEvent<TestParameters>>
{
    private readonly ITestService _service;

    public TestCqrsEventHandler(ITestService service,
                                ILogger<TestCqrsEventHandler> logger,
                                IValidator<CqrsEvent<TestParameters>> validator)
        : base(logger, validator)
        => _service = service;

    public override async Task HandleEvent(CqrsEvent<TestParameters> cqrsEvent, CancellationToken cancellationToken = default)
        => await _service.Receive(cqrsEvent.Params.Value);
}
