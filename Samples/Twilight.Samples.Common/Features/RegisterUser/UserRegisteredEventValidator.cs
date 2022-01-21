using FluentValidation;
using Twilight.CQRS.Events;

namespace Twilight.Samples.Common.Features.RegisterUser;

public sealed class UserRegisteredEventValidator : AbstractValidator<CqrsEvent<UserRegisteredEventParameters>>
{
    public UserRegisteredEventValidator()
    {
        RuleFor(p => p.Params).NotNull();
        RuleFor(p => p.Params.Forename).NotNull().NotEmpty();
        RuleFor(p => p.Params.Surname).NotNull().NotEmpty();
        RuleFor(p => p.Params.UserId).NotNull().NotEmpty();
    }
}
