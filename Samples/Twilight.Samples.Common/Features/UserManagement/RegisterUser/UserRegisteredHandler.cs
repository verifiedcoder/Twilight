using Microsoft.Extensions.Logging;
using System.Diagnostics;
using Twilight.CQRS.Events;
using Twilight.Samples.Common.Data;
using Twilight.Samples.Common.Data.Entities;

namespace Twilight.Samples.Common.Features.UserManagement.RegisterUser;

[UsedImplicitly]
public sealed class UserRegisteredHandler(
    ViewDataContext dataContext,
    ILogger<UserRegisteredHandler> logger,
    IValidator<CqrsEvent<UserRegisteredParameters>> validator)
    : CqrsEventHandlerBase<UserRegisteredHandler, CqrsEvent<UserRegisteredParameters>>(logger, validator)
{
    public override async Task<Result> HandleEvent(CqrsEvent<UserRegisteredParameters> cqrsEvent, CancellationToken cancellationToken = default)
    {
        var userViewEntity = new UserViewEntity
        {
            UserId = cqrsEvent.Params.UserId,
            Forename = cqrsEvent.Params.Forename,
            Surname = cqrsEvent.Params.Surname,
            FullName = $"{cqrsEvent.Params.Surname}, {cqrsEvent.Params.Forename}",
            RegistrationDate = DateTimeOffset.UtcNow
        };

        using (var activity = Activity.Current?.Source.StartActivity(ActivityKind.Server))
        {
            activity?.AddEvent(new ActivityEvent("Add User to View"));

            dataContext.UsersView.Add(userViewEntity);
        }

        await dataContext.SaveChangesAsync(cancellationToken);

        if (Logger.IsEnabled(LogLevel.Information))
        {
            Logger.LogInformation("User Registered Handler: Handled Event, {EventTypeName}.", cqrsEvent.GetType().FullName);
        }

        return Result.Ok();
    }
}
