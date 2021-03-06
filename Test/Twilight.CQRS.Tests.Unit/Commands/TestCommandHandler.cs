﻿using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Twilight.CQRS.Commands;
using Twilight.CQRS.Events;
using Twilight.CQRS.Messaging.Contracts;
using Twilight.CQRS.Tests.Unit.Shared;

namespace Twilight.CQRS.Tests.Unit.Commands
{
    public sealed class TestCommandHandler : CommandHandlerBase<Command<TestParameters>>
    {
        public TestCommandHandler(IMessageSender messageSender,
                                  ILogger<TestCommandHandler> logger,
                                  IValidator<Command<TestParameters>> validator)
            : base(messageSender, logger, validator)
        {
        }

        protected override async Task HandleCommand(Command<TestParameters> command, CancellationToken cancellationToken = default)
        {
            await MessageSender.Publish(new Event<TestParameters>(command.Params, command.CorrelationId, command.MessageId), cancellationToken);

            await Task.CompletedTask;
        }
    }
}
