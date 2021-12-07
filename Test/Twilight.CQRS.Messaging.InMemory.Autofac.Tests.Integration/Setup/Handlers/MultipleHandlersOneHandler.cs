using FluentValidation;
using Microsoft.Extensions.Logging;
using Twilight.CQRS.Commands;
using Twilight.CQRS.Messaging.InMemory.Autofac.Tests.Integration.Setup.Parameters;
using Twilight.CQRS.Messaging.Interfaces;

namespace Twilight.CQRS.Messaging.InMemory.Autofac.Tests.Integration.Setup.Handlers;

public sealed class MultipleHandlersOneHandler : CommandHandlerBase<MultipleHandlersOneHandler, Command<MultipleHandlersParameters>>
{
    public MultipleHandlersOneHandler(IMessageSender messageSender,
                                        ILogger<MultipleHandlersOneHandler> logger,
                                        IValidator<Command<MultipleHandlersParameters>> validator)
        : base(messageSender, logger, validator)
    {
    }

    public override Task HandleCommand(Command<MultipleHandlersParameters> command, CancellationToken cancellationToken = default)
        => throw new NotImplementedException();
}
