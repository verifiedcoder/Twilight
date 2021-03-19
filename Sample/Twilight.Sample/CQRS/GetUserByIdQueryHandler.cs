using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Twilight.CQRS.Contracts;
using Twilight.CQRS.Queries;
using Twilight.Sample.Data;

namespace Twilight.Sample.CQRS
{
    public sealed class GetUserByIdQueryHandler : QueryHandlerBase<Query<GetUserByIdQueryParameters, QueryResponse<GetUserByIdQueryResponsePayload>>, QueryResponse<GetUserByIdQueryResponsePayload>>
    {
        private readonly UsersViewContext _context;
        private readonly ILogger<IMessageHandler<Query<GetUserByIdQueryParameters, QueryResponse<GetUserByIdQueryResponsePayload>>>> _logger;

        public GetUserByIdQueryHandler(UsersViewContext context,
                                       ILogger<IMessageHandler<Query<GetUserByIdQueryParameters, QueryResponse<GetUserByIdQueryResponsePayload>>>> logger,
                                       IValidator<Query<GetUserByIdQueryParameters, QueryResponse<GetUserByIdQueryResponsePayload>>> validator)
            : base(validator)
        {
            _logger = logger;
            _context = context;
        }

        protected override async Task<QueryResponse<GetUserByIdQueryResponsePayload>> HandleQuery(Query<GetUserByIdQueryParameters, QueryResponse<GetUserByIdQueryResponsePayload>> query, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Handled query, {QueryTypeName}.", query.GetType().FullName);

            var user = await _context.UsersView.FindAsync(new object[] {query.Params.UserId}, cancellationToken);

            var payload = new GetUserByIdQueryResponsePayload(user.Id, user.Forename, user.Surname);
            var response = new QueryResponse<GetUserByIdQueryResponsePayload>(payload, query.CorrelationId, query.MessageId);

            return await Task.FromResult(response);
        }
    }
}
