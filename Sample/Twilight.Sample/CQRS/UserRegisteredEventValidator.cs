using FluentValidation;
using Twilight.CQRS.Events;

namespace Twilight.Sample.CQRS
{
    public sealed class UserRegisteredEventValidator : AbstractValidator<Event<UserRegisteredEventParameters>>
    {
    }
}
