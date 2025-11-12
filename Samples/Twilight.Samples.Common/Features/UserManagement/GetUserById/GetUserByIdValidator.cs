using Twilight.CQRS.Queries;

namespace Twilight.Samples.Common.Features.UserManagement.GetUserById;

[UsedImplicitly]
public sealed class GetUserByIdValidator : AbstractValidator<CqrsQuery<GetUserByIdParameters, QueryResponse<GetUserByIdResponse>>>
{
    public GetUserByIdValidator()
    {
        RuleFor(p => p.Params).NotNull();
        RuleFor(p => p.Params.UserId).GreaterThan(0);
    }
}
