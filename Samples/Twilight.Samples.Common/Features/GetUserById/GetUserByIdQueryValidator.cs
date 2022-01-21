using FluentValidation;
using Twilight.CQRS.Queries;

namespace Twilight.Samples.Common.Features.GetUserById;

public sealed class GetUserByIdQueryValidator : AbstractValidator<CqrsQuery<GetUserByIdQueryParameters, QueryResponse<GetUserByIdQueryResponsePayload>>>
{
    public GetUserByIdQueryValidator()
    {
        RuleFor(p => p.Params).NotNull();
        RuleFor(p => p.Params.UserId).GreaterThan(0);
    }
}
