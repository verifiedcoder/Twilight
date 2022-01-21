using FluentValidation;
using Twilight.CQRS.Commands;

namespace Twilight.Samples.Common.Features.RegisterUser;

public sealed class RegisterUserCommandValidator : AbstractValidator<CqrsCommand<RegisterUserCommandParameters>>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(p => p.Params).NotNull();
        RuleFor(p => p.Params.Forename).NotNull().NotEmpty();
        RuleFor(p => p.Params.Surname).NotNull().NotEmpty();
    }
}
