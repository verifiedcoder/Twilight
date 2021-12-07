using FluentValidation;
using Twilight.CQRS.Commands;
using Twilight.CQRS.Tests.Unit.Common;

namespace Twilight.CQRS.Messaging.InMemory.Autofac.Tests.Integration.Setup.Validators;

public sealed class TestCommandValidator : AbstractValidator<Command<TestParameters>>
{
}
