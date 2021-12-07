using FluentValidation;
using Twilight.CQRS.Queries;

namespace Twilight.Samples.CQRS.Features.GetUserById;

public sealed class GetUserByIdQueryValidator : AbstractValidator<Query<GetUserByIdQueryParameters, QueryResponse<GetUserByIdQueryResponsePayload>>>
{
    public GetUserByIdQueryValidator()
        => RuleFor(p => p.Params.UserId).GreaterThan(0);
}
