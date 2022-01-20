using FluentValidation;
using Twilight.CQRS.Events;
using Twilight.CQRS.Tests.Common;

namespace Twilight.CQRS.Messaging.InMemory.Autofac.Tests.Integration.Setup.Validators;

internal sealed class TestEventValidator : AbstractValidator<CqrsEvent<TestParameters>>
{
}
