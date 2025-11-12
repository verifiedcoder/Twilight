using Microsoft.Extensions.Logging;
using Twilight.CQRS.Commands;
using Twilight.CQRS.Messaging.Interfaces;

namespace Twilight.CQRS.Messaging.InMemory.Autofac.Tests.Integration.Setup.Handlers;

[UsedImplicitly]
internal sealed class MultipleHandlersSecondHandler(
    IMessageSender messageSender,
    ILogger<MultipleHandlersFirstHandler> logger,
    IValidator<CqrsCommand<MultipleHandlersParameters>> validator) : CqrsCommandHandlerBase<MultipleHandlersFirstHandler, CqrsCommand<MultipleHandlersParameters>>(messageSender, logger, validator)
{
    public override Task<Result> HandleCommand(CqrsCommand<MultipleHandlersParameters> command, CancellationToken cancellationToken = default)
        => throw new NotImplementedException();
}
