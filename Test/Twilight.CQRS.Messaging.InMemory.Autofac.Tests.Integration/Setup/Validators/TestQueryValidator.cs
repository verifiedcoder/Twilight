using FluentValidation;
using Twilight.CQRS.Queries;
using Twilight.CQRS.Tests.Common;

namespace Twilight.CQRS.Messaging.InMemory.Autofac.Tests.Integration.Setup.Validators;

internal sealed class TestQueryValidator : AbstractValidator<CqrsQuery<TestParameters, QueryResponse<string>>>
{
}
