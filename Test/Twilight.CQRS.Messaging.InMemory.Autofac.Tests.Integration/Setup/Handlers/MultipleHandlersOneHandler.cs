using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Twilight.CQRS.Commands;
using Twilight.CQRS.Messaging.Contracts;
using Twilight.CQRS.Messaging.InMemory.Autofac.Tests.Integration.Setup.Parameters;

namespace Twilight.CQRS.Messaging.InMemory.Autofac.Tests.Integration.Setup.Handlers
{
    public sealed class MultipleHandlersOneHandler : CommandHandlerBase<Command<MultipleHandlersParameters>>
    {
        public MultipleHandlersOneHandler(IMessageSender messageSender,
                                          IValidator<Command<MultipleHandlersParameters>> validator)
            : base(messageSender, validator)
        {
        }

        protected override Task HandleCommand(Command<MultipleHandlersParameters> command, CancellationToken cancellationToken = default) => throw new NotImplementedException();
    }
}
