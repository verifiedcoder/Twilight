using FluentValidation;
using Twilight.CQRS.Events;

namespace Twilight.Samples.CQRS.Features.RegisterUser;

public sealed class UserRegisteredEventValidator : AbstractValidator<Event<UserRegisteredEventParameters>>
{
}
