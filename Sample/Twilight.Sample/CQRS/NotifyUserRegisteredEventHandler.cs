using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Twilight.CQRS.Contracts;
using Twilight.CQRS.Events;

namespace Twilight.Sample.CQRS
{
    public sealed class NotifyUserRegisteredEventHandler : EventHandlerBase<Event<UserRegisteredEventParameters>>
    {
        private readonly ILogger<IMessageHandler<Event<UserRegisteredEventParameters>>> _logger;

        public NotifyUserRegisteredEventHandler(ILogger<IMessageHandler<Event<UserRegisteredEventParameters>>> logger,
                                                IValidator<Event<UserRegisteredEventParameters>> validator)
            : base(validator) => _logger = logger;

        protected override async Task HandleEvent(Event<UserRegisteredEventParameters> @event, CancellationToken cancellationToken = default)
        {
            // Events can be used to trigger multiple business activities.
            _logger.LogInformation("Notify User Registered Handler: Handled event, {EventTypeName}.", @event.GetType().FullName);

            await Task.CompletedTask;
        }
    }
}
