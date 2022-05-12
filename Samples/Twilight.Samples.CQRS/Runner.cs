using System.Diagnostics;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Taikandi;
using Twilight.CQRS.Commands;
using Twilight.CQRS.Messaging.Interfaces;
using Twilight.CQRS.Queries;
using Twilight.Samples.Common;
using Twilight.Samples.Common.Features.GetUserById;
using Twilight.Samples.Common.Features.GetUsersView;
using Twilight.Samples.Common.Features.RegisterUser;

namespace Twilight.Samples.CQRS;

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
        using var activity = Activity.Current?.Source.StartActivity($"{nameof(RegisterUser)}");
        {
            var id = activity?.Id ?? SequentialGuid.NewGuid().ToString();
            var parameters = new RegisterUserCommandParameters("Bilbo", "Baggins");
            var command = new CqrsCommand<RegisterUserCommandParameters>(parameters, id);

            await _messageSender.Send(command);
        }
    }

    private async Task GetRegisteredUser()
    {
        using var activity = Activity.Current?.Source.StartActivity($"{nameof(GetRegisteredUser)}");
        {
            var id = activity?.Id ?? SequentialGuid.NewGuid().ToString();
            var parameters = new GetUserByIdQueryParameters(1);
            var query = new CqrsQuery<GetUserByIdQueryParameters, QueryResponse<GetUserByIdQueryResponsePayload>>(parameters, id);

            var response = await _messageSender.Send(query);

            _logger.LogInformation("Query response: {@GetRegisteredUserResponse}", response);
        }
    }

    private async Task GetUsersView()
    {
        using var activity = Activity.Current?.Source.StartActivity($"{nameof(GetUsersView)}");
        {
            var id = activity?.Id ?? SequentialGuid.NewGuid().ToString();
            var parameters = new GetUsersViewQueryParameters(DateTimeOffset.UtcNow.AddDays(-1));
            var query = new CqrsQuery<GetUsersViewQueryParameters, QueryResponse<GetUsersViewQueryResponsePayload>>(parameters, id);

            var response = await _messageSender.Send(query);

            _logger.LogInformation("Query response: {@GetUsersViewResponse}", response);
        }
    }

    private async Task GetInvalidUsersView()
    {
        using var activity = Activity.Current?.Source.StartActivity($"{nameof(GetInvalidUsersView)}");
        {
            var id = activity?.Id ?? SequentialGuid.NewGuid().ToString();
            var parameters = new GetUsersViewQueryParameters(DateTimeOffset.UtcNow.AddDays(+1));
            var query = new CqrsQuery<GetUsersViewQueryParameters, QueryResponse<GetUsersViewQueryResponsePayload>>(parameters, id);

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
