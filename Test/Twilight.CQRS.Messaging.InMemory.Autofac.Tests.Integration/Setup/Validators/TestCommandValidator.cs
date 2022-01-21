using FluentValidation;
using Twilight.CQRS.Commands;
using Twilight.CQRS.Tests.Common;

namespace Twilight.CQRS.Messaging.InMemory.Autofac.Tests.Integration.Setup.Validators;

internal sealed class TestCommandValidator : AbstractValidator<CqrsCommand<TestParameters>>
{
}
