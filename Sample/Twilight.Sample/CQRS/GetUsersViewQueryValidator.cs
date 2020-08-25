using System;
using FluentValidation;
using Twilight.CQRS.Queries;

namespace Twilight.Sample.CQRS
{
    public sealed class GetUsersViewQueryValidator : AbstractValidator<Query<GetUsersViewQueryParameters, QueryResponse<GetUsersViewQueryResponsePayload>>>
    {
        public GetUsersViewQueryValidator()
        {
            RuleFor(p => p.Params.RegistrationDate).LessThanOrEqualTo(DateTimeOffset.UtcNow);
        }
    }
}
