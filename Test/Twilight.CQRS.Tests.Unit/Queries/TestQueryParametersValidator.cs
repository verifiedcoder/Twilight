using FluentValidation;
using Twilight.CQRS.Queries;
using Twilight.CQRS.Tests.Common;

namespace Twilight.CQRS.Tests.Unit.Queries;

internal sealed class TestQueryParametersValidator : AbstractValidator<CqrsQuery<TestParameters, QueryResponse<TestQueryResponse>>>
{
    public TestQueryParametersValidator()
        => RuleFor(p => p.Params.Value).NotEmpty();
}
