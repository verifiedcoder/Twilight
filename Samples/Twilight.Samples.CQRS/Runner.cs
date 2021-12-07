using FluentValidation;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Reflection;
using Taikandi;
using Twilight.CQRS.Commands;
using Twilight.CQRS.Messaging.Interfaces;
using Twilight.CQRS.Queries;
using Twilight.Samples.CQRS.Features.GetUserById;
using Twilight.Samples.CQRS.Features.GetUsersView;
using Twilight.Samples.CQRS.Features.RegisterUser;

namespace Twilight.Samples.CQRS;

internal sealed class Runner : IRunner
{
    const string defaultAssemblyVersion = "1.0.0.0";

    private readonly ILogger<Runner> _logger;
    private readonly ActivitySource _activitySource;
    private readonly IMessageSender _messageSender;

    private string ActivitySourceName => typeof(Runner).Namespace ?? nameof(Runner);

    private static string AssemblyVersion => Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? defaultAssemblyVersion;

    public Runner(IMessageSender messageSender, ILogger<Runner> logger)
    {
        _messageSender = messageSender;
        _logger = logger;
        _activitySource = new ActivitySource(ActivitySourceName, AssemblyVersion);
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
        using var activity = _activitySource.StartActivity($"{nameof(RegisterUser)}");
        {
            var parameters = new RegisterUserCommandParameters("Bilbo", "Baggins");
            var command = new Command<RegisterUserCommandParameters>(parameters, SequentialGuid.NewGuid().ToString());

            await _messageSender.Send(command);
        }
    }

    private async Task GetRegisteredUser()
    {
        using var activity = _activitySource.StartActivity($"{nameof(GetRegisteredUser)}");
        {
            var parameters = new GetUserByIdQueryParameters(1);
            var query = new Query<GetUserByIdQueryParameters, QueryResponse<GetUserByIdQueryResponsePayload>>(parameters, SequentialGuid.NewGuid().ToString());

            var response = await _messageSender.Send(query);

            _logger.LogInformation("Query response: {@GetRegisteredUserResponse}", response);
        }
    }

    private async Task GetUsersView()
    {
        using var activity = _activitySource.StartActivity($"{nameof(GetUsersView)}");
        {
            var parameters = new GetUsersViewQueryParameters(DateTimeOffset.UtcNow.AddDays(-1));
            var query = new Query<GetUsersViewQueryParameters, QueryResponse<GetUsersViewQueryResponsePayload>>(parameters, SequentialGuid.NewGuid().ToString());

            var response = await _messageSender.Send(query);

            _logger.LogInformation("Query response: {@GetUsersViewResponse}", response);
        }
    }

    private async Task GetInvalidUsersView()
    {
        using var activity = _activitySource.StartActivity($"{nameof(GetInvalidUsersView)}");
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
