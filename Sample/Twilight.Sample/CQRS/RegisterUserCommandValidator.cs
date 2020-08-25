using FluentValidation;
using Twilight.CQRS.Commands;

namespace Twilight.Sample.CQRS
{
    public sealed class RegisterUserCommandValidator : AbstractValidator<Command<RegisterUserCommandParameters>>
    {
    }
}
