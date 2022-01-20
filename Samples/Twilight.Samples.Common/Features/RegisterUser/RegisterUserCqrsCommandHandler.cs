using FluentValidation;
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

        // ReSharper disable once MethodHasAsyncOverloadWithCancellation
        var entityEntry = _context.Users.Add(userEntity);

        await _context.SaveChangesAsync(cancellationToken);

        var parameters = new UserRegisteredEventParameters(entityEntry.Entity.Id, cqrsCommand.Params.Forename, cqrsCommand.Params.Surname);
        var userRegisteredEvent = new CqrsEvent<UserRegisteredEventParameters>(parameters, cqrsCommand.CorrelationId, cqrsCommand.MessageId);

        Logger.LogInformation("Handled Command, {CommandTypeName}.", cqrsCommand.GetType().FullName);

        await MessageSender.Publish(userRegisteredEvent, cancellationToken);
    }
}
