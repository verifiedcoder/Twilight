using FluentValidation;
using Twilight.CQRS.Commands;
using Twilight.CQRS.Messaging.InMemory.Autofac.Tests.Integration.Setup.Parameters;

namespace Twilight.CQRS.Messaging.InMemory.Autofac.Tests.Integration.Setup.Validators;

internal sealed class MultipleHandlersValidator : AbstractValidator<CqrsCommand<MultipleHandlersParameters>>
{
}
