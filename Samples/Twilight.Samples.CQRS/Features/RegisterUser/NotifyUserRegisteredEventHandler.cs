using FluentValidation;
using Microsoft.Extensions.Logging;
using Twilight.CQRS.Events;

namespace Twilight.Samples.CQRS.Features.RegisterUser;

public sealed class NotifyUserRegisteredEventHandler : EventHandlerBase<NotifyUserRegisteredEventHandler, Event<UserRegisteredEventParameters>>
{
    public NotifyUserRegisteredEventHandler(ILogger<NotifyUserRegisteredEventHandler> logger,
                                            IValidator<Event<UserRegisteredEventParameters>> validator)
        : base(logger, validator)
    {
    }

    public override async Task HandleEvent(Event<UserRegisteredEventParameters> @event, CancellationToken cancellationToken = default)
    {
        // Events can be used to trigger multiple business activities.
        Logger.LogInformation("Notify User Registered Handler: Handled event, {EventTypeName}.", @event.GetType().FullName);

        await Task.CompletedTask;
    }
}
