using FluentValidation;
using Twilight.CQRS.Commands;
using Twilight.CQRS.Tests.Unit.Shared;

namespace Twilight.CQRS.Tests.Unit
{
    public sealed class TestParametersValidator : AbstractValidator<Command<TestParameters>>
    {
        public TestParametersValidator()
        {
            RuleFor(p => p.Params.Value).NotEmpty();
        }
    }
}
