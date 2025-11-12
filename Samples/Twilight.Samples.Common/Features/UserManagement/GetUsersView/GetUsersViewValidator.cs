using Twilight.CQRS.Queries;

namespace Twilight.Samples.Common.Features.UserManagement.GetUsersView;

[UsedImplicitly]
public sealed class GetUsersViewValidator : AbstractValidator<CqrsQuery<GetUsersViewParameters, QueryResponse<GetUsersViewResponse>>>
{
    public GetUsersViewValidator()
    {
        RuleFor(p => p.Params).NotNull();
        RuleFor(p => p.Params.RegistrationDate).LessThanOrEqualTo(DateTimeOffset.UtcNow);
    }
}
