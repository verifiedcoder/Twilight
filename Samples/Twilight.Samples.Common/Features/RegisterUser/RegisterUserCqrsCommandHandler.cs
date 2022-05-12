using System.Diagnostics;
using FluentValidation;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using Twilight.CQRS.Commands;
using Twilight.CQRS.Events;
using Twilight.CQRS.Messaging.Interfaces;
using Twilight.Samples.Common.Data;
using Twilight.Samples.Common.Data.Entities;

namespace Twilight.Samples.Common.Features.RegisterUser;

public sealed class RegisterUserCqrsCommandHandler : CqrsCommandHandlerBase<RegisterUserCqrsCommandHandler, CqrsCommand<RegisterUserCommandParameters>>
{
    private readonly SampleDataContext _context;

    public RegisterUserCqrsCommandHandler(SampleDataContext context,
                                          IMessageSender messageSender,
                                          ILogger<RegisterUserCqrsCommandHandler> logger,
                                          IValidator<CqrsCommand<RegisterUserCommandParameters>> validator)
        : base(messageSender, logger, validator)
        => _context = context;

    public override async Task HandleCommand(CqrsCommand<RegisterUserCommandParameters> cqrsCommand, CancellationToken cancellationToken = default)
    {
        var userEntity = new UserEntity
        {
            Forename = cqrsCommand.Params.Forename,
            Surname = cqrsCommand.Params.Surname
        };

        EntityEntry<UserEntity> entityEntry;

        using (var activity = Activity.Current?.Source.StartActivity("Adding new user to database", ActivityKind.Server))
        {
            activity?.AddEvent(new ActivityEvent("Register User"));

            entityEntry = _context.Users.Add(userEntity);

            await _context.SaveChangesAsync(cancellationToken);
        }

        var parameters = new UserRegisteredEventParameters(entityEntry.Entity.Id, cqrsCommand.Params.Forename, cqrsCommand.Params.Surname);
        var userRegisteredEvent = new CqrsEvent<UserRegisteredEventParameters>(parameters, cqrsCommand.CorrelationId, cqrsCommand.MessageId);

        Logger.LogInformation("Handled CQRS Command, {CommandTypeName}.", cqrsCommand.GetType().FullName);

        await MessageSender.Publish(userRegisteredEvent, cancellationToken);
    }
}
