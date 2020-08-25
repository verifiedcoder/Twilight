using FluentValidation;
using Twilight.CQRS.Commands;
using Twilight.CQRS.Tests.Unit.Shared;

namespace Twilight.CQRS.Tests.Unit.Commands
{
    public sealed class TestCommandParametersValidator : AbstractValidator<Command<TestParameters>>
    {
        public TestCommandParametersValidator()
        {
            RuleFor(p => p.Params.Value).NotEmpty();
        }
    }
}
