using FluentResults;
using System.Diagnostics;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Twilight.CQRS.Events;
using Twilight.Samples.Common.Data;
using Twilight.Samples.Common.Data.Entities;

namespace Twilight.Samples.Common.Features.RegisterUser;

public sealed class UserRegisteredCqrsEventHandler(ViewDataContext dataContext,
                                                   ILogger<UserRegisteredCqrsEventHandler> logger,
                                                   IValidator<CqrsEvent<UserRegisteredEventParameters>> validator)
    : CqrsEventHandlerBase<UserRegisteredCqrsEventHandler, CqrsEvent<UserRegisteredEventParameters>>(logger, validator)
{
    public override async Task<Result> HandleEvent(CqrsEvent<UserRegisteredEventParameters> cqrsEvent, CancellationToken cancellationToken = default)
    {
        var userViewEntity = new UserViewEntity
        {
            UserId = cqrsEvent.Params.UserId,
            Forename = cqrsEvent.Params.Forename,
            Surname = cqrsEvent.Params.Surname,
            FullName = $"{cqrsEvent.Params.Surname}, {cqrsEvent.Params.Forename}",
            RegistrationDate = DateTimeOffset.UtcNow
        };

        using (var activity = Activity.Current?.Source.StartActivity("Adding new user to users view", ActivityKind.Server))
        {
            activity?.AddEvent(new ActivityEvent("Add User to View"));

            dataContext.UsersView.Add(userViewEntity);
        }


        await dataContext.SaveChangesAsync(cancellationToken);

        Logger.LogInformation("User Registered Handler: Handled Event, {EventTypeName}.", cqrsEvent.GetType().FullName);

        return Result.Ok();
    }
}
