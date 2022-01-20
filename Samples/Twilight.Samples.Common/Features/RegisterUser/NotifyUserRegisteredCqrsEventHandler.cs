using FluentValidation;
using Microsoft.Extensions.Logging;
using Twilight.CQRS.Events;

namespace Twilight.Samples.Common.Features.RegisterUser;

public sealed class NotifyUserRegisteredCqrsEventHandler : CqrsEventHandlerBase<NotifyUserRegisteredCqrsEventHandler, CqrsEvent<UserRegisteredEventParameters>>
{
    public NotifyUserRegisteredCqrsEventHandler(ILogger<NotifyUserRegisteredCqrsEventHandler> logger,
                                                IValidator<CqrsEvent<UserRegisteredEventParameters>> validator)
        : base(logger, validator)
    {
    }

    public override async Task HandleEvent(CqrsEvent<UserRegisteredEventParameters> cqrsEvent, CancellationToken cancellationToken = default)
    {
        // Events can be used to trigger multiple business activities.
        Logger.LogInformation("Notify User Registered Handler: Handled Event, {EventTypeName}.", cqrsEvent.GetType().FullName);

        await Task.CompletedTask;
    }
}
