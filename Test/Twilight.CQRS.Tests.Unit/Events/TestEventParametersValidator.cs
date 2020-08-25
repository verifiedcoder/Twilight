using FluentValidation;
using Twilight.CQRS.Events;
using Twilight.CQRS.Tests.Unit.Shared;

namespace Twilight.CQRS.Tests.Unit.Events
{
    public sealed class TestEventParametersValidator : AbstractValidator<Event<TestParameters>>
    {
        public TestEventParametersValidator()
        {
            RuleFor(p => p.Params.Value).NotEmpty();
        }
    }
}
