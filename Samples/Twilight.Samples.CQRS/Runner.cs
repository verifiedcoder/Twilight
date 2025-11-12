using FluentResults;
using Serilog;
using System.Diagnostics;
using Taikandi;
using Twilight.CQRS.Commands;
using Twilight.CQRS.Messaging.Interfaces;
using Twilight.CQRS.Queries;
using Twilight.Samples.Common;
using Twilight.Samples.Common.Features.UserManagement.GetUserById;
using Twilight.Samples.Common.Features.UserManagement.GetUsersView;
using Twilight.Samples.Common.Features.UserManagement.RegisterUser;

namespace Twilight.Samples.CQRS;

[UsedImplicitly]
internal sealed class Runner(IMessageSender messageSender) : IRunner
{
    public async Task Run()
    {
        await DemoRegisterUser();
        await DemoGetRegisteredUser();
        await DemoGetUsersView();
        await DemoGetInvalidUsersView();
    }

    private async Task DemoRegisterUser()
    {
        using var activity = Activity.Current?.Source.StartActivity($"{nameof(DemoRegisterUser)}");

        var id = GetActivityId(activity);
        var parameters = new RegisterUserCommandParameters("Bilbo", "Baggins");
        var command = new CqrsCommand<RegisterUserCommandParameters>(parameters, id);

        await messageSender.Send(command);
    }

    private async Task DemoGetRegisteredUser()
    {
        using var activity = Activity.Current?.Source.StartActivity($"{nameof(DemoGetRegisteredUser)}");

        var id = GetActivityId(activity);
        var parameters = new GetUserByIdParameters(1);
        var query = new CqrsQuery<GetUserByIdParameters, QueryResponse<GetUserByIdResponse>>(parameters, id);

        var response = await messageSender.Send(query);

        Log.Information("Query response: {@GetRegisteredUserResponse}", response);
    }

    private async Task DemoGetUsersView()
    {
        using var activity = Activity.Current?.Source.StartActivity($"{nameof(DemoGetUsersView)}");

        var id = GetActivityId(activity);
        var parameters = new GetUsersViewParameters(DateTimeOffset.UtcNow.AddDays(-1));
        var query = new CqrsQuery<GetUsersViewParameters, QueryResponse<GetUsersViewResponse>>(parameters, id);

        var response = await messageSender.Send(query);

        Log.Information("Query response: {@GetUsersViewResponse}", response);
    }

    private async Task DemoGetInvalidUsersView()
    {
        using var activity = Activity.Current?.Source.StartActivity($"{nameof(DemoGetInvalidUsersView)}");

        var id = GetActivityId(activity);
        var parameters = new GetUsersViewParameters(DateTimeOffset.UtcNow.AddDays(+1));
        var query = new CqrsQuery<GetUsersViewParameters, QueryResponse<GetUsersViewResponse>>(parameters, id);

        var response = await messageSender.Send(query);

        if (response.IsFailed)
        {
            LogValidationErrors(response.Errors);
        }
    }

    private static string GetActivityId(Activity? activity) 
        => activity?.Id ?? SequentialGuid.NewGuid().ToString();

    private static void LogValidationErrors(IEnumerable<IError> errors)
    {
        Log.Error("Query validation failed:");

        foreach (var error in errors)
        {
            Log.Error(" - {ValidationError}", error);
        }
    }
}
