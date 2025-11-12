using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Twilight.CQRS.Queries;
using Twilight.Samples.Common.Data;
using Twilight.Samples.Common.Data.Entities;

namespace Twilight.Samples.Common.Features.UserManagement.GetUserById;

public sealed record GetUserByIdParameters(int UserId);

public sealed record GetUserByIdResponse(int UserId, string Forename, string Surname);

[UsedImplicitly]
public sealed class GetUserByIdHandler(
    ViewDataContext dataContext,
    ILogger<GetUserByIdHandler> logger,
    IValidator<CqrsQuery<GetUserByIdParameters, QueryResponse<GetUserByIdResponse>>> validator) 
    : CqrsQueryHandlerBase<GetUserByIdHandler, CqrsQuery<GetUserByIdParameters, QueryResponse<GetUserByIdResponse>>, QueryResponse<GetUserByIdResponse>>(logger, validator)
{
    protected override async Task<Result<QueryResponse<GetUserByIdResponse>>> HandleQuery(CqrsQuery<GetUserByIdParameters, QueryResponse<GetUserByIdResponse>> query, CancellationToken cancellationToken = default)
    {
        UserViewEntity? userView;

        using (var activity = Activity.Current?.Source.StartActivity(ActivityKind.Server))
        {
            activity?.AddEvent(new ActivityEvent("Get User by ID"));
            activity?.SetTag(nameof(GetUserByIdParameters.UserId), query.Params.UserId);

            userView = await dataContext.UsersView.FindAsync([query.Params.UserId], cancellationToken);
        }

        if (userView == null)
        {
            return Result.Fail($"User with Id '{query.Params.UserId}' not found.");
        }

        var payload = new GetUserByIdResponse(userView.Id, userView.Forename, userView.Surname);
        var response = new QueryResponse<GetUserByIdResponse>(payload, query.CorrelationId, null, query.MessageId);

        if (Logger.IsEnabled(LogLevel.Information))
        {
            Logger.LogInformation("Handled CQRS Query, {QueryTypeName}.", query.GetType().FullName);
        }

        return await Task.FromResult(Result.Ok(response));
    }
}
