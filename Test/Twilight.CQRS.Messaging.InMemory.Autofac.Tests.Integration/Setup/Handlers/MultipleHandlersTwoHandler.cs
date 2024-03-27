using FluentResults;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Twilight.CQRS.Commands;
using Twilight.CQRS.Messaging.InMemory.Autofac.Tests.Integration.Setup.Parameters;
using Twilight.CQRS.Messaging.Interfaces;

namespace Twilight.CQRS.Messaging.InMemory.Autofac.Tests.Integration.Setup.Handlers;

internal sealed class MultipleHandlersTwoHandler(
    IMessageSender messageSender,
    ILogger<MultipleHandlersOneHandler> logger,
    IValidator<CqrsCommand<MultipleHandlersParameters>> validator)
    : CqrsCommandHandlerBase<MultipleHandlersOneHandler, CqrsCommand<MultipleHandlersParameters>>(messageSender, logger, validator)
{
    public override Task<Result> HandleCommand(CqrsCommand<MultipleHandlersParameters> command, CancellationToken cancellationToken = default)
        => throw new NotImplementedException();
}
