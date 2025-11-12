using Twilight.CQRS.Commands;

namespace Twilight.Samples.Common.Features.UserManagement.RegisterUser;

[UsedImplicitly]
public sealed class RegisterUserValidator : AbstractValidator<CqrsCommand<RegisterUserCommandParameters>>
{
    public RegisterUserValidator()
    {
        RuleFor(p => p.Params).NotNull();
        RuleFor(p => p.Params.Forename).NotNull().NotEmpty();
        RuleFor(p => p.Params.Surname).NotNull().NotEmpty();
    }
}
