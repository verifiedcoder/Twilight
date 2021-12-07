using FluentValidation;
using Microsoft.Extensions.Logging;
using Twilight.CQRS.Events;
using Twilight.Samples.CQRS.Data;
using Twilight.Samples.CQRS.Data.Entities;

namespace Twilight.Samples.CQRS.Features.RegisterUser;

public sealed class UserRegisteredEventHandler : EventHandlerBase<UserRegisteredEventHandler, Event<UserRegisteredEventParameters>>
{
    private readonly ViewDataContext _dataContext;

    public UserRegisteredEventHandler(ViewDataContext dataContext,
                                        ILogger<UserRegisteredEventHandler> logger,
                                        IValidator<Event<UserRegisteredEventParameters>> validator)
        : base(logger, validator)
        => _dataContext = dataContext;

    public override async Task HandleEvent(Event<UserRegisteredEventParameters> @event, CancellationToken cancellationToken = default)
    {
        var userViewEntity = new UserViewEntity
        {
            UserId = @event.Params.UserId,
            Forename = @event.Params.Forename,
            Surname = @event.Params.Surname,
            FullName = $"{@event.Params.Surname}, {@event.Params.Forename}",
            RegistrationDate = DateTimeOffset.UtcNow
        };

        // ReSharper disable once MethodHasAsyncOverloadWithCancellation
        _dataContext.UsersView.Add(userViewEntity);

        await _dataContext.SaveChangesAsync(cancellationToken);

        Logger.LogInformation("User Registered Handler: Handled event, {EventTypeName}.", @event.GetType().FullName);

        await Task.CompletedTask;
    }
}
