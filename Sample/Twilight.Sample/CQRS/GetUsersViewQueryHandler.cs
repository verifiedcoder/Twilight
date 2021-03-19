using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Twilight.CQRS.Contracts;
using Twilight.CQRS.Queries;
using Twilight.Sample.Data;
using Twilight.Sample.Views;

namespace Twilight.Sample.CQRS
{
    public sealed class GetUsersViewQueryHandler : QueryHandlerBase<Query<GetUsersViewQueryParameters, QueryResponse<GetUsersViewQueryResponsePayload>>, QueryResponse<GetUsersViewQueryResponsePayload>>
    {
        private readonly UsersViewContext _context;
        private readonly ILogger<IMessageHandler<Query<GetUsersViewQueryParameters, QueryResponse<GetUsersViewQueryResponsePayload>>>> _logger;

        public GetUsersViewQueryHandler(UsersViewContext context,
                                        ILogger<IMessageHandler<Query<GetUsersViewQueryParameters, QueryResponse<GetUsersViewQueryResponsePayload>>>> logger,
                                        IValidator<Query<GetUsersViewQueryParameters, QueryResponse<GetUsersViewQueryResponsePayload>>> validator)
            : base(validator)
        {
            _logger = logger;
            _context = context;
        }

        protected override async Task<QueryResponse<GetUsersViewQueryResponsePayload>> HandleQuery(Query<GetUsersViewQueryParameters, QueryResponse<GetUsersViewQueryResponsePayload>> query, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Handled query, {QueryTypeName}.", query.GetType().FullName);

            var userViewEntities = await _context.UsersView.Where(u => u.RegistrationDate >= query.Params.RegistrationDate)
                                                 .OrderBy(v => v.RegistrationDate)
                                                 .ToListAsync(cancellationToken);

            var usersView = userViewEntities.Select(u => new UserView(u.Id, u.UserId, u.Forename, u.Surname, u.FullName, u.RegistrationDate));

            var payload = new GetUsersViewQueryResponsePayload(usersView);
            var response = new QueryResponse<GetUsersViewQueryResponsePayload>(payload, query.CorrelationId, query.MessageId);

            return await Task.FromResult(response);
        }
    }
}
