using FluentValidation;
using Microsoft.Extensions.Logging;
using Twilight.CQRS.Commands;
using Twilight.CQRS.Messaging.InMemory.Autofac.Tests.Integration.Setup.Parameters;
using Twilight.CQRS.Messaging.Interfaces;

namespace Twilight.CQRS.Messaging.InMemory.Autofac.Tests.Integration.Setup.Handlers;

internal sealed class MultipleHandlersOneHandler : CqrsCommandHandlerBase<MultipleHandlersOneHandler, CqrsCommand<MultipleHandlersParameters>>
{
    public MultipleHandlersOneHandler(IMessageSender messageSender,
                                      ILogger<MultipleHandlersOneHandler> logger,
                                      IValidator<CqrsCommand<MultipleHandlersParameters>> validator)
        : base(messageSender, logger, validator)
    {
    }

    public override Task HandleCommand(CqrsCommand<MultipleHandlersParameters> cqrsCommand, CancellationToken cancellationToken = default)
        => throw new NotImplementedException();
}
