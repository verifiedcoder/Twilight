using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Twilight.CQRS.Events;
using Twilight.CQRS.Tests.Unit.Shared;

namespace Twilight.CQRS.Tests.Unit.Events
{
    public sealed class TestEventHandler : EventHandlerBase<Event<TestParameters>>
    {
        public TestEventHandler(IValidator<Event<TestParameters>> validator)
            : base(validator)
        {
        }

        protected override async Task HandleEvent(Event<TestParameters> command, CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
        }
    }
}
