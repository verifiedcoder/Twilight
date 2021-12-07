using FluentValidation;
using Twilight.CQRS.Queries;
using Twilight.CQRS.Tests.Unit.Common;

namespace Twilight.CQRS.Tests.Unit.Queries;

public sealed class TestQueryParametersValidator : AbstractValidator<Query<TestParameters, QueryResponse<TestQueryResponse>>>
{
    public TestQueryParametersValidator()
        => RuleFor(p => p.Params.Value).NotEmpty();
}
