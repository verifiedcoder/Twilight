using FluentValidation;
using Microsoft.Extensions.Logging;
using Twilight.CQRS.Events;
using Twilight.CQRS.Tests.Unit.Common;

namespace Twilight.CQRS.Messaging.InMemory.Autofac.Tests.Integration.Setup.Handlers;

public sealed class TestEventHandler : EventHandlerBase<TestEventHandler, Event<TestParameters>>
{
    private readonly ITestService _service;

    public TestEventHandler(ITestService service,
                            ILogger<TestEventHandler> logger,
                            IValidator<Event<TestParameters>> validator)
        : base(logger, validator)
        => _service = service;

    public override async Task HandleEvent(Event<TestParameters> @event, CancellationToken cancellationToken = default)
        => await _service.Receive(@event.Params.Value);
}
