using FluentResults;
using System.Diagnostics;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Twilight.CQRS.Queries;
using Twilight.Samples.Common.Data;
using Twilight.Samples.Common.Data.Entities;

namespace Twilight.Samples.Common.Features.GetUserById;

public sealed class GetUserByIdCqrsQueryHandler(ViewDataContext dataContext,
                                                ILogger<GetUserByIdCqrsQueryHandler> logger,
                                                IValidator<CqrsQuery<GetUserByIdQueryParameters, QueryResponse<GetUserByIdQueryResponsePayload>>> validator)
    : CqrsQueryHandlerBase<GetUserByIdCqrsQueryHandler, CqrsQuery<GetUserByIdQueryParameters, QueryResponse<GetUserByIdQueryResponsePayload>>, QueryResponse<GetUserByIdQueryResponsePayload>>(logger, validator)
{
    protected override async Task<Result<QueryResponse<GetUserByIdQueryResponsePayload>>> HandleQuery(CqrsQuery<GetUserByIdQueryParameters, QueryResponse<GetUserByIdQueryResponsePayload>> query, CancellationToken cancellationToken = default)
    {
        UserViewEntity? userView;

        using (var activity = Activity.Current?.Source.StartActivity("Getting user from view", ActivityKind.Server))
        {
            activity?.AddEvent(new ActivityEvent("Get User by ID"));
            activity?.SetTag(nameof(GetUserByIdQueryParameters.UserId), query.Params.UserId);

            userView = await dataContext.UsersView.FindAsync([query.Params.UserId], cancellationToken);
        }

        if (userView == null)
        {
            return Result.Fail($"User with Id '{query.Params.UserId}' not found.");
        }
        
        var payload = new GetUserByIdQueryResponsePayload(userView.Id, userView.Forename, userView.Surname);
        var response = new QueryResponse<GetUserByIdQueryResponsePayload>(payload, query.CorrelationId, null, query.MessageId);

        Logger.LogInformation("Handled CQRS Query, {QueryTypeName}.", query.GetType().FullName);

        return await Task.FromResult(Result.Ok(response));
    }
}
