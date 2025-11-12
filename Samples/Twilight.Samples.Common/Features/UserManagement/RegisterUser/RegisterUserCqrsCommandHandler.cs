using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using Twilight.CQRS.Commands;
using Twilight.CQRS.Events;
using Twilight.CQRS.Messaging.Interfaces;
using Twilight.Samples.Common.Data;
using Twilight.Samples.Common.Data.Entities;

namespace Twilight.Samples.Common.Features.UserManagement.RegisterUser;

public sealed record RegisterUserCommandParameters(string Forename, string Surname);

[UsedImplicitly]
public sealed class RegisterUserCqrsCommandHandler(
    SampleDataContext context,
    IMessageSender messageSender,
    ILogger<RegisterUserCqrsCommandHandler> logger,
    IValidator<CqrsCommand<RegisterUserCommandParameters>> validator)
    : CqrsCommandHandlerBase<RegisterUserCqrsCommandHandler, CqrsCommand<RegisterUserCommandParameters>>(messageSender, logger, validator)
{
    public override async Task<Result> HandleCommand(CqrsCommand<RegisterUserCommandParameters> command, CancellationToken cancellationToken = default)
    {
        var userEntity = new UserEntity
        {
            Forename = command.Params.Forename,
            Surname = command.Params.Surname
        };

        EntityEntry<UserEntity> entityEntry;

        using (var activity = Activity.Current?.Source.StartActivity(ActivityKind.Server))
        {
            activity?.AddEvent(new ActivityEvent("Register User"));

            entityEntry = context.Users.Add(userEntity);

            await context.SaveChangesAsync(cancellationToken);
        }

        var parameters = new UserRegisteredParameters(entityEntry.Entity.Id, command.Params.Forename, command.Params.Surname);
        var userRegisteredEvent = new CqrsEvent<UserRegisteredParameters>(parameters, command.CorrelationId, null, command.MessageId);

        if (Logger.IsEnabled(LogLevel.Information))
        {
            Logger.LogInformation("Handled CQRS Command, {CommandTypeName}.", command.GetType().FullName);
        }

        await MessageSender.Publish(userRegisteredEvent, cancellationToken);

        return Result.Ok();
    }
}
