using FluentResults;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Twilight.CQRS.Events;

namespace Twilight.Samples.Common.Features.RegisterUser;

public sealed class NotifyUserRegisteredCqrsEventHandler(
    ILogger<NotifyUserRegisteredCqrsEventHandler> logger,
    IValidator<CqrsEvent<UserRegisteredEventParameters>> validator) : CqrsEventHandlerBase<NotifyUserRegisteredCqrsEventHandler, CqrsEvent<UserRegisteredEventParameters>>(logger, validator)
{
    public override async Task<Result> HandleEvent(CqrsEvent<UserRegisteredEventParameters> cqrsEvent, CancellationToken cancellationToken = default)
    {
        // Events can be used to trigger multiple business activities.
        Logger.LogInformation("Notify User Registered Handler: Handled Event, {EventTypeName}.", cqrsEvent.GetType().FullName);

        return await Task.FromResult(Result.Ok());
    }
}
