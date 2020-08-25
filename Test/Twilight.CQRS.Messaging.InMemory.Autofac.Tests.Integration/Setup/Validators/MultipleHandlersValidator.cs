using FluentValidation;
using Twilight.CQRS.Commands;
using Twilight.CQRS.Messaging.InMemory.Autofac.Tests.Integration.Setup.Parameters;

namespace Twilight.CQRS.Messaging.InMemory.Autofac.Tests.Integration.Setup.Validators
{
    public sealed class MultipleHandlersValidator : AbstractValidator<Command<MultipleHandlersParameters>>
    {
    }
}
