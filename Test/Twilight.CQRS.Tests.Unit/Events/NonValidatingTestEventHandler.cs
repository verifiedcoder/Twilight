using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Twilight.CQRS.Events;

namespace Twilight.CQRS.Tests.Unit.Events
{
    public sealed class NonValidatingTestEventHandler : EventHandlerBase<Event<string>>
    {
        public NonValidatingTestEventHandler(ILogger<NonValidatingTestEventHandler> logger)
            : base(logger)
        {
        }

        protected override async Task HandleEvent(Event<string> command, CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
        }
    }
}
