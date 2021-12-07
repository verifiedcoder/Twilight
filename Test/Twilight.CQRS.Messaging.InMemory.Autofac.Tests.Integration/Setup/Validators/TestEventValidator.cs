using FluentValidation;
using Twilight.CQRS.Events;
using Twilight.CQRS.Tests.Unit.Common;

namespace Twilight.CQRS.Messaging.InMemory.Autofac.Tests.Integration.Setup.Validators;

public sealed class TestEventValidator : AbstractValidator<Event<TestParameters>>
{
}
