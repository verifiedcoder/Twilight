using FluentValidation;
using Microsoft.Extensions.Logging;
using Twilight.CQRS.Commands;
using Twilight.CQRS.Events;
using Twilight.CQRS.Messaging.Interfaces;
using Twilight.Samples.CQRS.Data;
using Twilight.Samples.CQRS.Data.Entities;

namespace Twilight.Samples.CQRS.Features.RegisterUser;

public sealed class RegisterUserCommandHandler : CommandHandlerBase<RegisterUserCommandHandler, Command<RegisterUserCommandParameters>>
{
    private readonly SampleDataContext _context;

    public RegisterUserCommandHandler(SampleDataContext context,
                                        IMessageSender messageSender,
                                        ILogger<RegisterUserCommandHandler> logger,
                                        IValidator<Command<RegisterUserCommandParameters>> validator)
        : base(messageSender, logger, validator)
        => _context = context;

    public override async Task HandleCommand(Command<RegisterUserCommandParameters> command, CancellationToken cancellationToken = default)
    {
        Logger.LogInformation("Handled command, {CommandTypeName}.", command.GetType().FullName);

        var userEntity = new UserEntity
        {
            Forename = command.Params.Forename,
            Surname = command.Params.Surname
        };

        // ReSharper disable once MethodHasAsyncOverloadWithCancellation
        var entityEntry = _context.Users.Add(userEntity);

        await _context.SaveChangesAsync(cancellationToken);

        var parameters = new UserRegisteredEventParameters(entityEntry.Entity.Id, command.Params.Forename, command.Params.Surname);
        var userRegisteredEvent = new Event<UserRegisteredEventParameters>(parameters, command.CorrelationId, command.MessageId);

        await MessageSender.Publish(userRegisteredEvent, cancellationToken);

        await Task.CompletedTask;
    }
}
