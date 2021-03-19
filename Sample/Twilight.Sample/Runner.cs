using System;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Taikandi;
using Twilight.CQRS.Commands;
using Twilight.CQRS.Messaging.Contracts;
using Twilight.CQRS.Queries;
using Twilight.Sample.CQRS;

namespace Twilight.Sample
{
    internal sealed class Runner : IRunner
    {
        private readonly ILogger<Runner> _logger;
        private readonly IMessageSender _messageSender;

        public Runner(IMessageSender messageSender, ILogger<Runner> logger)
        {
            _messageSender = messageSender;
            _logger = logger;
        }

        public async Task Run()
        {
            await RegisterUser();
            await GetRegisteredUser();
            await GetUsersView();
            await GetInvalidUsersView();
        }

        private async Task RegisterUser()
        {
            var parameters = new RegisterUserCommandParameters("Bilbo", "Baggins");
            var command = new Command<RegisterUserCommandParameters>(parameters, SequentialGuid.NewGuid().ToString());

            await _messageSender.Send(command);
        }

        private async Task GetRegisteredUser()
        {
            var parameters = new GetUserByIdQueryParameters(1);
            var query = new Query<GetUserByIdQueryParameters, QueryResponse<GetUserByIdQueryResponsePayload>>(parameters, SequentialGuid.NewGuid().ToString());

            var response = await _messageSender.Send(query);

            _logger.LogInformation("Query response: {@GetRegisteredUserResponse}", response);
        }

        private async Task GetUsersView()
        {
            var parameters = new GetUsersViewQueryParameters(DateTimeOffset.UtcNow.AddDays(-1));
            var query = new Query<GetUsersViewQueryParameters, QueryResponse<GetUsersViewQueryResponsePayload>>(parameters, SequentialGuid.NewGuid().ToString());

            var response = await _messageSender.Send(query);

            _logger.LogInformation("Query response: {@GetUsersViewResponse}", response);
        }

        private async Task GetInvalidUsersView()
        {
            var parameters = new GetUsersViewQueryParameters(DateTimeOffset.UtcNow.AddDays(+1));
            var query = new Query<GetUsersViewQueryParameters, QueryResponse<GetUsersViewQueryResponsePayload>>(parameters, SequentialGuid.NewGuid().ToString());

            try
            {
                // No point assigning the response as this call has been engineered to throw a ValidationException
                await _messageSender.Send(query);
            }
            catch (ValidationException validationException)
            {
                _logger.LogError("Query validation failed:");

                foreach (var validationError in validationException.Errors)
                {
                    _logger.LogError(" - {ValidationError}", validationError);
                }
            }
        }
    }
}
