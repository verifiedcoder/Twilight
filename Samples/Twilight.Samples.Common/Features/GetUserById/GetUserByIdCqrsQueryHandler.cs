using System.Diagnostics;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Twilight.CQRS.Exceptions;
using Twilight.CQRS.Queries;
using Twilight.Samples.Common.Data;
using Twilight.Samples.Common.Data.Entities;

namespace Twilight.Samples.Common.Features.GetUserById;

public sealed class GetUserByIdCqrsQueryHandler : CqrsQueryHandlerBase<GetUserByIdCqrsQueryHandler, CqrsQuery<GetUserByIdQueryParameters, QueryResponse<GetUserByIdQueryResponsePayload>>, QueryResponse<GetUserByIdQueryResponsePayload>>
{
    private readonly ViewDataContext _dataContext;

    public GetUserByIdCqrsQueryHandler(ViewDataContext dataContext,
                                       ILogger<GetUserByIdCqrsQueryHandler> logger,
                                       IValidator<CqrsQuery<GetUserByIdQueryParameters, QueryResponse<GetUserByIdQueryResponsePayload>>> validator)
        : base(logger, validator)
        => _dataContext = dataContext;

    protected override async Task<QueryResponse<GetUserByIdQueryResponsePayload>> HandleQuery(CqrsQuery<GetUserByIdQueryParameters, QueryResponse<GetUserByIdQueryResponsePayload>> cqrsQuery, CancellationToken cancellationToken = default)
    {
        UserViewEntity? userView;

        using (var activity = Activity.Current?.Source.StartActivity("Getting user from view", ActivityKind.Server))
        {
            activity?.AddEvent(new ActivityEvent("Get User by ID"));
            activity?.SetTag(nameof(GetUserByIdQueryParameters.UserId), cqrsQuery.Params.UserId);

            userView = await _dataContext.UsersView.FindAsync(new object[] { cqrsQuery.Params.UserId }, cancellationToken);
        }

        if (userView == null)

        {
            throw new HandlerException($"User with Id '{cqrsQuery.Params.UserId}' not found.");
        }

        var payload = new GetUserByIdQueryResponsePayload(userView.Id, userView.Forename, userView.Surname);
        var response = new QueryResponse<GetUserByIdQueryResponsePayload>(payload, cqrsQuery.CorrelationId, cqrsQuery.MessageId);

        Logger.LogInformation("Handled CQRS Query, {QueryTypeName}.", cqrsQuery.GetType().FullName);

        return await Task.FromResult(response);
    }
}
