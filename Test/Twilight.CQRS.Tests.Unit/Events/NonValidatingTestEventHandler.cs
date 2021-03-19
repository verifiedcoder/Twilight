using System.Threading;
using System.Threading.Tasks;
using Twilight.CQRS.Events;

namespace Twilight.CQRS.Tests.Unit.Events
{
    public sealed class NonValidatingTestEventHandler : EventHandlerBase<Event<string>>
    {
        protected override async Task HandleEvent(Event<string> command, CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
        }
    }
}
