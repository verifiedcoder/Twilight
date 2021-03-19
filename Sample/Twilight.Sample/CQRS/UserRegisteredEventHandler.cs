using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Twilight.CQRS.Contracts;
using Twilight.CQRS.Events;
using Twilight.Sample.Data;
using Twilight.Sample.Data.Entities;

namespace Twilight.Sample.CQRS
{
    public sealed class UserRegisteredEventHandler : EventHandlerBase<Event<UserRegisteredEventParameters>>
    {
        private readonly UsersViewContext _context;
        private readonly ILogger<IMessageHandler<Event<UserRegisteredEventParameters>>> _logger;

        public UserRegisteredEventHandler(UsersViewContext context,
                                          ILogger<IMessageHandler<Event<UserRegisteredEventParameters>>> logger,
                                          IValidator<Event<UserRegisteredEventParameters>> validator)
            : base(validator)
        {
            _logger = logger;
            _context = context;
        }

        protected override async Task HandleEvent(Event<UserRegisteredEventParameters> @event, CancellationToken cancellationToken = default)
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
            _context.UsersView.Add(userViewEntity);

            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("User Registered Handler: Handled event, {EventTypeName}.", @event.GetType().FullName);

            await Task.CompletedTask;
        }
    }
}
