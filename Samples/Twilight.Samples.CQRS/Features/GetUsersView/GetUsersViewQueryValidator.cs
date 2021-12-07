using FluentValidation;
using Twilight.CQRS.Queries;

namespace Twilight.Samples.CQRS.Features.GetUsersView;

public sealed class GetUsersViewQueryValidator : AbstractValidator<Query<GetUsersViewQueryParameters, QueryResponse<GetUsersViewQueryResponsePayload>>>
{
    public GetUsersViewQueryValidator()
        => RuleFor(p => p.Params.RegistrationDate).LessThanOrEqualTo(DateTimeOffset.UtcNow);
}
