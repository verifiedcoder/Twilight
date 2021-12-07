using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Twilight.CQRS.Queries;
using Twilight.Samples.CQRS.Data;
using Twilight.Samples.CQRS.Views;

namespace Twilight.Samples.CQRS.Features.GetUsersView;

public sealed class GetUsersViewQueryHandler : QueryHandlerBase<GetUsersViewQueryHandler, Query<GetUsersViewQueryParameters, QueryResponse<GetUsersViewQueryResponsePayload>>, QueryResponse<GetUsersViewQueryResponsePayload>>
{
    private readonly ViewDataContext _dataContext;

    public GetUsersViewQueryHandler(ViewDataContext dataContext,
                                    ILogger<GetUsersViewQueryHandler> logger,
                                    IValidator<Query<GetUsersViewQueryParameters, QueryResponse<GetUsersViewQueryResponsePayload>>> validator)
        : base(logger, validator)
        => _dataContext = dataContext;

    protected override async Task<QueryResponse<GetUsersViewQueryResponsePayload>> HandleQuery(Query<GetUsersViewQueryParameters, QueryResponse<GetUsersViewQueryResponsePayload>> query, CancellationToken cancellationToken = default)
    {
        Logger.LogInformation("Handled query, {QueryTypeName}.", query.GetType().FullName);

        var userViewEntities = await _dataContext.UsersView.Where(u => u.RegistrationDate >= query.Params.RegistrationDate)
                                                    .OrderBy(v => v.RegistrationDate)
                                                    .ToListAsync(cancellationToken);

        var usersView = userViewEntities.Select(u => new UserView(u.Id, u.UserId, u.Forename, u.Surname, u.FullName, u.RegistrationDate));

        var payload = new GetUsersViewQueryResponsePayload(usersView);
        var response = new QueryResponse<GetUsersViewQueryResponsePayload>(payload, query.CorrelationId, query.MessageId);

        return await Task.FromResult(response);
    }
}
