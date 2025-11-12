using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Twilight.CQRS.Queries;
using Twilight.Samples.Common.Data;
using Twilight.Samples.Common.Data.Entities;
using Twilight.Samples.Common.Views;

namespace Twilight.Samples.Common.Features.UserManagement.GetUsersView;

public sealed record GetUsersViewParameters(DateTimeOffset RegistrationDate);

public sealed record GetUsersViewResponse(IEnumerable<UserView> Users);

[UsedImplicitly]
public sealed class GetUsersViewHandler(
    ViewDataContext dataContext,
    ILogger<GetUsersViewHandler> logger,
    IValidator<CqrsQuery<GetUsersViewParameters, QueryResponse<GetUsersViewResponse>>> validator)
    : CqrsQueryHandlerBase<GetUsersViewHandler, CqrsQuery<GetUsersViewParameters, QueryResponse<GetUsersViewResponse>>, QueryResponse<GetUsersViewResponse>>(logger, validator)
{
    protected override async Task<Result<QueryResponse<GetUsersViewResponse>>> HandleQuery(CqrsQuery<GetUsersViewParameters, QueryResponse<GetUsersViewResponse>> query, CancellationToken cancellationToken = default)
    {
        List<UserViewEntity>? userViews;

        using (var activity = Activity.Current?.Source.StartActivity(ActivityKind.Server))
        {
            activity?.AddEvent(new ActivityEvent("Get All Users"));

            userViews = await dataContext.UsersView.Where(u => u.RegistrationDate >= query.Params.RegistrationDate)
                                         .OrderBy(v => v.RegistrationDate)
                                         .ToListAsync(cancellationToken);
        }

        var usersView = userViews.Select(u => new UserView(u.Id, u.UserId, u.Forename, u.Surname, u.FullName, u.RegistrationDate));

        var payload = new GetUsersViewResponse(usersView);
        var response = new QueryResponse<GetUsersViewResponse>(payload, query.CorrelationId, null, query.MessageId);

        if (Logger.IsEnabled(LogLevel.Information))
        {
            Logger.LogInformation("Handled CQRS Query, {QueryTypeName}.", query.GetType().FullName);
        }

        return await Task.FromResult(Result.Ok(response));
    }
}
