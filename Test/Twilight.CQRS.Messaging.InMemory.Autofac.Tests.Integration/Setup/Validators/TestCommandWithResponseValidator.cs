using FluentValidation;
using Twilight.CQRS.Commands;
using Twilight.CQRS.Tests.Unit.Common;

namespace Twilight.CQRS.Messaging.InMemory.Autofac.Tests.Integration.Setup.Validators;

public sealed class TestCommandWithResponseValidator : AbstractValidator<Command<TestParameters, CommandResponse<string>>>
{
}
