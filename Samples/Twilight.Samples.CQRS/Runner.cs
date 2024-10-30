using Serilog;
using System.Diagnostics;
using Taikandi;
using Twilight.CQRS.Commands;
using Twilight.CQRS.Messaging.Interfaces;
using Twilight.CQRS.Queries;
using Twilight.Samples.Common;
using Twilight.Samples.Common.Features.GetUserById;
using Twilight.Samples.Common.Features.GetUsersView;
using Twilight.Samples.Common.Features.RegisterUser;

namespace Twilight.Samples.CQRS;

internal sealed class Runner(IMessageSender messageSender) : IRunner
{
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

            await messageSender.Send(command);
        }
    }

    private async Task GetRegisteredUser()
    {
        using var activity = Activity.Current?.Source.StartActivity($"{nameof(GetRegisteredUser)}");
        {
            var id = activity?.Id ?? SequentialGuid.NewGuid().ToString();
            var parameters = new GetUserByIdQueryParameters(1);
            var query = new CqrsQuery<GetUserByIdQueryParameters, QueryResponse<GetUserByIdQueryResponsePayload>>(parameters, id);

            var response = await messageSender.Send(query);

            Log.Information("Query response: {@GetRegisteredUserResponse}", response);
        }
    }

    private async Task GetUsersView()
    {
        using var activity = Activity.Current?.Source.StartActivity($"{nameof(GetUsersView)}");
        {
            var id = activity?.Id ?? SequentialGuid.NewGuid().ToString();
            var parameters = new GetUsersViewQueryParameters(DateTimeOffset.UtcNow.AddDays(-1));
            var query = new CqrsQuery<GetUsersViewQueryParameters, QueryResponse<GetUsersViewQueryResponsePayload>>(parameters, id);

            var response = await messageSender.Send(query);

            Log.Information("Query response: {@GetUsersViewResponse}", response);
        }
    }

    private async Task GetInvalidUsersView()
    {
        using var activity = Activity.Current?.Source.StartActivity($"{nameof(GetInvalidUsersView)}");
        {
            var id = activity?.Id ?? SequentialGuid.NewGuid().ToString();
            var parameters = new GetUsersViewQueryParameters(DateTimeOffset.UtcNow.AddDays(+1));
            var query = new CqrsQuery<GetUsersViewQueryParameters, QueryResponse<GetUsersViewQueryResponsePayload>>(parameters, id);

            var response = await messageSender.Send(query);

            if (response.IsFailed)
            {
                Log.Error("Query validation failed:");

                foreach (var error in response.Errors)
                {
                    Log.Error(" - {ValidationError}", error);
                }
            }
        }
    }
}
