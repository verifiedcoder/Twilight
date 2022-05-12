using System.Diagnostics;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Twilight.CQRS.Queries;
using Twilight.Samples.Common.Data;
using Twilight.Samples.Common.Data.Entities;
using Twilight.Samples.Common.Views;

namespace Twilight.Samples.Common.Features.GetUsersView;

public sealed class GetUsersViewCqrsQueryHandler : CqrsQueryHandlerBase<GetUsersViewCqrsQueryHandler, CqrsQuery<GetUsersViewQueryParameters, QueryResponse<GetUsersViewQueryResponsePayload>>, QueryResponse<GetUsersViewQueryResponsePayload>>
{
    private readonly ViewDataContext _dataContext;

    public GetUsersViewCqrsQueryHandler(ViewDataContext dataContext,
                                        ILogger<GetUsersViewCqrsQueryHandler> logger,
                                        IValidator<CqrsQuery<GetUsersViewQueryParameters, QueryResponse<GetUsersViewQueryResponsePayload>>> validator)
        : base(logger, validator)
        => _dataContext = dataContext;

    protected override async Task<QueryResponse<GetUsersViewQueryResponsePayload>> HandleQuery(CqrsQuery<GetUsersViewQueryParameters, QueryResponse<GetUsersViewQueryResponsePayload>> cqrsQuery, CancellationToken cancellationToken = default)
    {
        List<UserViewEntity>? userViews;

        using (var activity = Activity.Current?.Source.StartActivity("Get all users in view", ActivityKind.Server))
        {
            activity?.AddEvent(new ActivityEvent("Get All Users"));

            userViews = await _dataContext.UsersView.Where(u => u.RegistrationDate >= cqrsQuery.Params.RegistrationDate)
                                          .OrderBy(v => v.RegistrationDate)
                                          .ToListAsync(cancellationToken);
        }

        var usersView = userViews.Select(u => new UserView(u.Id, u.UserId, u.Forename, u.Surname, u.FullName, u.RegistrationDate));

        var payload = new GetUsersViewQueryResponsePayload(usersView);
        var response = new QueryResponse<GetUsersViewQueryResponsePayload>(payload, cqrsQuery.CorrelationId, cqrsQuery.MessageId);

        Logger.LogInformation("Handled CQRS Query, {QueryTypeName}.", cqrsQuery.GetType().FullName);

        return await Task.FromResult(response);
    }
}
