using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Twilight.CQRS.Commands;
using Twilight.CQRS.Contracts;
using Twilight.CQRS.Events;
using Twilight.CQRS.Messaging.Contracts;
using Twilight.Sample.Data;
using Twilight.Sample.Data.Entities;

namespace Twilight.Sample.CQRS
{
    public sealed class RegisterUserCommandHandler : CommandHandlerBase<Command<RegisterUserCommandParameters>>
    {
        private readonly UsersContext _context;
        private readonly ILogger<IMessageHandler<Command<RegisterUserCommandParameters>>> _logger;

        public RegisterUserCommandHandler(UsersContext context,
                                          IMessageSender messageSender,
                                          ILogger<IMessageHandler<Command<RegisterUserCommandParameters>>> logger,
                                          IValidator<Command<RegisterUserCommandParameters>> validator)
            : base(messageSender, validator)
        {
            _logger = logger;
            _context = context;
        }

        protected override async Task HandleCommand(Command<RegisterUserCommandParameters> command, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Handled command, {CommandTypeName}.", command.GetType().FullName);

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
}
