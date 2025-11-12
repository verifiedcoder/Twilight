using Microsoft.Extensions.Logging;
using Twilight.CQRS.Events;

namespace Twilight.Samples.Common.Features.UserManagement.RegisterUser;

public sealed record UserRegisteredParameters(int UserId, string Forename, string Surname);

[UsedImplicitly]
public sealed class NotifyUserRegisteredHandler(
    ILogger<NotifyUserRegisteredHandler> logger,
    IValidator<CqrsEvent<UserRegisteredParameters>> validator) 
    : CqrsEventHandlerBase<NotifyUserRegisteredHandler, CqrsEvent<UserRegisteredParameters>>(logger, validator)
{
    public override async Task<Result> HandleEvent(CqrsEvent<UserRegisteredParameters> cqrsEvent, CancellationToken cancellationToken = default)
    {
        if (Logger.IsEnabled(LogLevel.Information))
        {
            // Events can be used to trigger multiple business activities.
            Logger.LogInformation("Notify User Registered Handler: Handled Event, {EventTypeName}.", cqrsEvent.GetType().FullName);
        }

        return await Task.FromResult(Result.Ok());
    }
}
