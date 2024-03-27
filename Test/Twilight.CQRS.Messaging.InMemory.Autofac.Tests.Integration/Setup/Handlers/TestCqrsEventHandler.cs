using FluentResults;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Twilight.CQRS.Events;
using Twilight.CQRS.Tests.Common;

namespace Twilight.CQRS.Messaging.InMemory.Autofac.Tests.Integration.Setup.Handlers;

internal sealed class TestCqrsEventHandler(ITestService service,
                                           ILogger<TestCqrsEventHandler> logger,
                                           IValidator<CqrsEvent<TestParameters>> validator) : CqrsEventHandlerBase<TestCqrsEventHandler, CqrsEvent<TestParameters>>(logger, validator)
{
    public override async Task<Result> HandleEvent(CqrsEvent<TestParameters> cqrsEvent, CancellationToken cancellationToken = default)
        => await service.Receive(cqrsEvent.Params.Value);
}
