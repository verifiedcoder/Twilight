using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using Twilight.CQRS.Commands;
using Twilight.CQRS.Contracts;
using Twilight.CQRS.Events;
using Twilight.CQRS.Messaging.InMemory.Autofac.Tests.Integration.Setup;
using Twilight.CQRS.Messaging.Shared;
using Twilight.CQRS.Queries;
using Twilight.CQRS.Tests.Unit.Shared;
using Xunit;

namespace Twilight.CQRS.Messaging.InMemory.Autofac.Tests.Integration
{
    public sealed class AutofacInMemoryMessageSenderTests : IntegrationTestBase
    {
        [Fact]
        public async Task MessageSenderCallsCorrectHandlerForCommand()
        {
            var command = new Command<TestParameters>(new TestParameters(nameof(MessageSenderCallsCorrectHandlerForCommand)), Constants.CorrelationId);

            await Subject.Send(command, CancellationToken.None);

            await Verifier.Received(1).Receive(Arg.Is(command.Params.Value));
        }

        [Fact]
        public async Task MessageSenderCallsCorrectHandlerForEvent()
        {
            var @event = new Event<TestParameters>(new TestParameters(nameof(MessageSenderCallsCorrectHandlerForEvent)), Constants.CorrelationId, Constants.CausationId);

            await Subject.Publish(@event, CancellationToken.None);

            await Verifier.Received(1).Receive(Arg.Is(@event.Params.Value));
        }

        [Fact]
        public async Task MessageSenderCallsCorrectHandlerForEvents()
        {
            var @event = new Event<TestParameters>(new TestParameters(nameof(MessageSenderCallsCorrectHandlerForEvents)), Constants.CorrelationId, Constants.CausationId);
            var events = new List<Event<TestParameters>>
                         {
                             @event
                         };

            var enumerableEvents = events.AsEnumerable();

            await Subject.Publish(enumerableEvents, CancellationToken.None);

            await Verifier.Received(1).Receive(@event.Params.Value);
        }

        [Fact]
        public async Task MessageSenderCallsCorrectHandlerForQuery()
        {
            var query = new Query<TestParameters, QueryResponse<string>>(new TestParameters(nameof(MessageSenderCallsCorrectHandlerForQuery)), Constants.CorrelationId);

            await Subject.Send(query, CancellationToken.None);

            await Verifier.Received(1).Receive(Arg.Is(query.Params.Value));
        }

        [Fact]
        public async Task MessageSenderThrowsWhenCommandHandlerIsNotFound()
        {
            var command = new Command<string>(string.Empty, Constants.CorrelationId, Constants.CausationId);
            var typeRef = typeof(ICommandHandler<Command<string>>).AssemblyQualifiedName;

            Func<Task> subjectResult = async () => { await Subject.Send(command, CancellationToken.None); };

            await subjectResult.Should().ThrowAsync<HandlerNotFoundException>()
                               .WithMessage($"No concrete handlers for type '{typeRef}' could be found.");
        }

        [Fact]
        public async Task MessageSenderThrowsWhenEventHandlerIsNotFound()
        {
            var @event = new Event<string>(string.Empty, Constants.CorrelationId, Constants.CausationId);
            var typeRef = typeof(IEventHandler<Event<string>>).AssemblyQualifiedName;

            Func<Task> subjectResult = async () => { await Subject.Publish(@event, CancellationToken.None); };

            await subjectResult.Should().ThrowAsync<HandlerNotFoundException>()
                               .WithMessage($"No concrete handlers for type '{typeRef}' could be found.");
        }

        [Fact]
        public async Task MessageSenderThrowsWhenQueryHandlerIsNotFound()
        {
            var query = new Query<object, QueryResponse<string>>(string.Empty, Constants.CorrelationId);
            var typeRef = typeof(IQueryHandler<Query<object, QueryResponse<string>>, QueryResponse<string>>).AssemblyQualifiedName;

            Func<Task> subjectResult = async () => { await Subject.Send(query, CancellationToken.None); };

            await subjectResult.Should().ThrowAsync<HandlerNotFoundException>()
                               .WithMessage($"No concrete handlers for type '{typeRef}' could be found.");
        }
    }
}
