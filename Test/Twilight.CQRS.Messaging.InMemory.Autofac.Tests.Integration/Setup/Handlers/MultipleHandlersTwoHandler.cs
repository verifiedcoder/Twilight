﻿using FluentValidation;
using Microsoft.Extensions.Logging;
using Twilight.CQRS.Commands;
using Twilight.CQRS.Messaging.InMemory.Autofac.Tests.Integration.Setup.Parameters;
using Twilight.CQRS.Messaging.Interfaces;

namespace Twilight.CQRS.Messaging.InMemory.Autofac.Tests.Integration.Setup.Handlers;

internal sealed class MultipleHandlersTwoHandler : CqrsCommandHandlerBase<MultipleHandlersOneHandler, CqrsCommand<MultipleHandlersParameters>>
{
    public MultipleHandlersTwoHandler(IMessageSender messageSender,
                                      ILogger<MultipleHandlersOneHandler> logger,
                                      IValidator<CqrsCommand<MultipleHandlersParameters>> validator)
        : base(messageSender, logger, validator)
    {
    }

    public override Task HandleCommand(CqrsCommand<MultipleHandlersParameters> command, CancellationToken cancellationToken = default)
        => throw new NotImplementedException();
}
