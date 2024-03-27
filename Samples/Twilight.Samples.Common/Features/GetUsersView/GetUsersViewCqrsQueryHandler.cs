using FluentResults;
using System.Diagnostics;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Twilight.CQRS.Queries;
using Twilight.Samples.Common.Data;
using Twilight.Samples.Common.Data.Entities;
using Twilight.Samples.Common.Views;

namespace Twilight.Samples.Common.Features.GetUsersView;

public sealed class GetUsersViewCqrsQueryHandler(ViewDataContext dataContext,
                                                 ILogger<GetUsersViewCqrsQueryHandler> logger,
                                                 IValidator<CqrsQuery<GetUsersViewQueryParameters, QueryResponse<GetUsersViewQueryResponsePayload>>> validator)
    : CqrsQueryHandlerBase<GetUsersViewCqrsQueryHandler, CqrsQuery<GetUsersViewQueryParameters, QueryResponse<GetUsersViewQueryResponsePayload>>, QueryResponse<GetUsersViewQueryResponsePayload>>(logger, validator)
{
    protected override async Task<Result<QueryResponse<GetUsersViewQueryResponsePayload>>> HandleQuery(CqrsQuery<GetUsersViewQueryParameters, QueryResponse<GetUsersViewQueryResponsePayload>> query, CancellationToken cancellationToken = default)
    {
        List<UserViewEntity>? userViews;

        using (var activity = Activity.Current?.Source.StartActivity("Get all users in view", ActivityKind.Server))
        {
            activity?.AddEvent(new ActivityEvent("Get All Users"));

            userViews = await dataContext.UsersView.Where(u => u.RegistrationDate >= query.Params.RegistrationDate)
                                          .OrderBy(v => v.RegistrationDate)
                                          .ToListAsync(cancellationToken);
        }

        var usersView = userViews.Select(u => new UserView(u.Id, u.UserId, u.Forename, u.Surname, u.FullName, u.RegistrationDate));

        var payload = new GetUsersViewQueryResponsePayload(usersView);
        var response = new QueryResponse<GetUsersViewQueryResponsePayload>(payload, query.CorrelationId, null, query.MessageId);

        Logger.LogInformation("Handled CQRS Query, {QueryTypeName}.", query.GetType().FullName);

        return await Task.FromResult(Result.Ok(response));
    }
}
