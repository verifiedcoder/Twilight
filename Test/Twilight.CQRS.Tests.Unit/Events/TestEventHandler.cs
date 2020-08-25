using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Twilight.CQRS.Events;
using Twilight.CQRS.Tests.Unit.Shared;

namespace Twilight.CQRS.Tests.Unit.Events
{
    public sealed class TestEventHandler : EventHandlerBase<Event<TestParameters>>
    {
        public TestEventHandler(ILogger<TestEventHandler> logger,
                                IValidator<Event<TestParameters>> validator)
            : base(logger, validator)
        {
        }

        protected override async Task HandleEvent(Event<TestParameters> command, CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
        }
    }
}
