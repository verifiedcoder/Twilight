using FluentValidation;
using Twilight.CQRS.Queries;

namespace Twilight.Samples.Common.Features.GetUsersView;

public sealed class GetUsersViewQueryValidator : AbstractValidator<CqrsQuery<GetUsersViewQueryParameters, QueryResponse<GetUsersViewQueryResponsePayload>>>
{
    public GetUsersViewQueryValidator()
    {
        RuleFor(p => p.Params).NotNull();
        RuleFor(p => p.Params.RegistrationDate).LessThanOrEqualTo(DateTimeOffset.UtcNow);
    }
}
