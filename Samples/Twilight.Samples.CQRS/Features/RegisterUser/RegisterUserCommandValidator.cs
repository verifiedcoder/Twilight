using FluentValidation;
using Twilight.CQRS.Commands;

namespace Twilight.Samples.CQRS.Features.RegisterUser;

public sealed class RegisterUserCommandValidator : AbstractValidator<Command<RegisterUserCommandParameters>>
{
}
