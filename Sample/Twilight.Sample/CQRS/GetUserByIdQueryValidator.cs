using FluentValidation;
using Twilight.CQRS.Queries;

namespace Twilight.Sample.CQRS
{
    public sealed class GetUserByIdQueryValidator : AbstractValidator<Query<GetUserByIdQueryParameters, QueryResponse<GetUserByIdQueryResponsePayload>>>
    {
        public GetUserByIdQueryValidator()
        {
            RuleFor(p => p.Params.UserId).GreaterThan(0);
        }
    }
}
