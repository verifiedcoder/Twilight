using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Twilight.CQRS.Commands;
using Twilight.CQRS.Messaging.Contracts;
using Twilight.CQRS.Messaging.InMemory.Autofac.Tests.Integration.Setup.Parameters;

namespace Twilight.CQRS.Messaging.InMemory.Autofac.Tests.Integration.Setup.Handlers
{
    public sealed class MultipleHandlersTwoHandler : CommandHandlerBase<Command<MultipleHandlersParameters>>
    {
        public MultipleHandlersTwoHandler(IMessageSender messageSender,
                                          ILogger<MultipleHandlersTwoHandler> logger,
                                          IValidator<Command<MultipleHandlersParameters>> validator)
            : base(messageSender, logger, validator)
        {
        }

        protected override Task HandleCommand(Command<MultipleHandlersParameters> command, CancellationToken cancellationToken = default) => throw new NotImplementedException();
    }
}
