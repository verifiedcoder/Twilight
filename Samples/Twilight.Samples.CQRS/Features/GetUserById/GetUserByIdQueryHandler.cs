using FluentValidation;
using Microsoft.Extensions.Logging;
using Twilight.CQRS.Exceptions;
using Twilight.CQRS.Queries;
using Twilight.Samples.CQRS.Data;

namespace Twilight.Samples.CQRS.Features.GetUserById;

public sealed class GetUserByIdQueryHandler : QueryHandlerBase<GetUserByIdQueryHandler, Query<GetUserByIdQueryParameters, QueryResponse<GetUserByIdQueryResponsePayload>>, QueryResponse<GetUserByIdQueryResponsePayload>>
{
    private readonly ViewDataContext _dataContext;

    public GetUserByIdQueryHandler(ViewDataContext dataContext,
                                    ILogger<GetUserByIdQueryHandler> logger,
                                    IValidator<Query<GetUserByIdQueryParameters, QueryResponse<GetUserByIdQueryResponsePayload>>> validator)
        : base(logger, validator)
        => _dataContext = dataContext;

    protected override async Task<QueryResponse<GetUserByIdQueryResponsePayload>> HandleQuery(Query<GetUserByIdQueryParameters, QueryResponse<GetUserByIdQueryResponsePayload>> query, CancellationToken cancellationToken = default)
    {
        Logger.LogInformation("Handled query, {QueryTypeName}.", query.GetType().FullName);

        var user = await _dataContext.UsersView.FindAsync(new object[] { query.Params.UserId }, cancellationToken);

        if (user == null)

        {
            throw new HandlerException($"User with Id '{query.Params.UserId}' not found.");
        }

        var payload = new GetUserByIdQueryResponsePayload(user.Id, user.Forename, user.Surname);
        var response = new QueryResponse<GetUserByIdQueryResponsePayload>(payload, query.CorrelationId, query.MessageId);

        return await Task.FromResult(response);
    }
}
