using FluentValidation;
using Microsoft.Extensions.Logging;
using Twilight.CQRS.Events;
using Twilight.Samples.Common.Data;
using Twilight.Samples.Common.Data.Entities;

namespace Twilight.Samples.Common.Features.RegisterUser;

public sealed class UserRegisteredCqrsEventHandler : CqrsEventHandlerBase<UserRegisteredCqrsEventHandler, CqrsEvent<UserRegisteredEventParameters>>
{
    private readonly ViewDataContext _dataContext;

    public UserRegisteredCqrsEventHandler(ViewDataContext dataContext,
                                          ILogger<UserRegisteredCqrsEventHandler> logger,
                                          IValidator<CqrsEvent<UserRegisteredEventParameters>> validator)
        : base(logger, validator)
        => _dataContext = dataContext;

    public override async Task HandleEvent(CqrsEvent<UserRegisteredEventParameters> cqrsEvent, CancellationToken cancellationToken = default)
    {
        var userViewEntity = new UserViewEntity
        {
            UserId = cqrsEvent.Params.UserId,
            Forename = cqrsEvent.Params.Forename,
            Surname = cqrsEvent.Params.Surname,
            FullName = $"{cqrsEvent.Params.Surname}, {cqrsEvent.Params.Forename}",
            RegistrationDate = DateTimeOffset.UtcNow
        };

        // ReSharper disable once MethodHasAsyncOverloadWithCancellation
        _dataContext.UsersView.Add(userViewEntity);

        await _dataContext.SaveChangesAsync(cancellationToken);

        Logger.LogInformation("User Registered Handler: Handled Event, {EventTypeName}.", cqrsEvent.GetType().FullName);
    }
}
