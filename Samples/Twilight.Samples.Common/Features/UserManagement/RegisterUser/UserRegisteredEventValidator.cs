using Twilight.CQRS.Events;

namespace Twilight.Samples.Common.Features.UserManagement.RegisterUser;

[UsedImplicitly]
public sealed class UserRegisteredEventValidator : AbstractValidator<CqrsEvent<UserRegisteredParameters>>
{
    public UserRegisteredEventValidator()
    {
        RuleFor(p => p.Params).NotNull();
        RuleFor(p => p.Params.Forename).NotNull().NotEmpty();
        RuleFor(p => p.Params.Surname).NotNull().NotEmpty();
        RuleFor(p => p.Params.UserId).NotNull().NotEmpty();
    }
}
