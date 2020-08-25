using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Twilight.CQRS.Commands;
using Twilight.CQRS.Contracts;
using Twilight.CQRS.Events;
using Twilight.CQRS.Messaging.InMemory.Autofac.Tests.Integration.Setup;
using Twilight.CQRS.Messaging.InMemory.Autofac.Tests.Integration.Setup.Parameters;
using Twilight.CQRS.Messaging.Shared;
using Twilight.CQRS.Tests.Unit.Shared;
using Xunit;

namespace Twilight.CQRS.Messaging.InMemory.Autofac.Tests.Integration
{
    public sealed class AutofacInMemoryMessageSenderExceptionTests : IntegrationTestBase
    {
        [Fact]
        public async Task MessageSenderThrowsWhenCommandHandlerDoesNotExist()
        {
            var command = new Command(Constants.CorrelationId);

            Func<Task> subjectResult = async () => { await Subject.Send(command, CancellationToken.None); };

            await subjectResult.Should().ThrowAsync<HandlerNotFoundException>()
                               .WithMessage($"No concrete handlers for type '{typeof(ICommandHandler<Command>).AssemblyQualifiedName}' could be found.");
        }

        [Fact]
        public async Task MessageSenderThrowsWhenMultipleCommandHandlersResolved()
        {
            var parameters = new MultipleHandlersParameters();
            var command = new Command<MultipleHandlersParameters>(parameters, Constants.CorrelationId);

            Func<Task> subjectResult = async () => { await Subject.Send(command, CancellationToken.None); };

            await subjectResult.Should().ThrowAsync<MultipleCommandHandlersDefinedException>()
                               .WithMessage($"Multiple command handlers for type '{typeof(ICommandHandler<Command<MultipleHandlersParameters>>).AssemblyQualifiedName}' were found. Only one handler can exist for a command.");
        }

        [Fact]
        public async Task MessageSenderThrowsWhenNoHandlerRegisteredForEvent()
        {
            var @event = new Event<string>(string.Empty, Constants.CorrelationId, Constants.CausationId);

            Func<Task> subjectResult = async () => { await Subject.Publish(@event, CancellationToken.None); };

            await subjectResult.Should().ThrowAsync<HandlerNotFoundException>()
                               .WithMessage($"No concrete handlers for type '{typeof(IEventHandler<Event<string>>).AssemblyQualifiedName}' could be found.");
        }
    }
}
