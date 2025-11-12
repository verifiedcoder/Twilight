using Twilight.CQRS.Commands;
using Twilight.CQRS.Tests.Common;

namespace Twilight.CQRS.Tests.Unit;

internal sealed class TestParametersValidator : AbstractValidator<CqrsCommand<TestParameters>>
{
    public TestParametersValidator()
        => RuleFor(p => p.Params.Value).NotEmpty();
}
