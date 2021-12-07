using FluentAssertions;
using NSubstitute;
using Twilight.CQRS.Commands;
using Twilight.CQRS.Events;
using Twilight.CQRS.Interfaces;
using Twilight.CQRS.Messaging.Common;
using Twilight.CQRS.Messaging.InMemory.Autofac.Tests.Integration.Setup;
using Twilight.CQRS.Messaging.InMemory.Autofac.Tests.Integration.Setup.Handlers;
using Twilight.CQRS.Queries;
using Twilight.CQRS.Tests.Unit.Common;
using Xunit;

namespace Twilight.CQRS.Messaging.InMemory.Autofac.Tests.Integration;

public sealed class AutofacInMemoryMessageSenderTests : IntegrationTestBase
{
    [Fact]
    public async Task MessageSenderCallsCorrectHandlerForCommand()
    {
        // Arrange
        var command = new Command<TestParameters>(new TestParameters(nameof(MessageSenderCallsCorrectHandlerForCommand)), Constants.CorrelationId);

        // Act
        await Subject.Send(command, CancellationToken.None);

        // Assert
        await Verifier.Received(1).Receive(Arg.Is(command.Params.Value));
    }

    [Fact]
    public async Task MessageSenderCallsCorrectHandlerForCommandWithResponse()
    {
        // Arrange
        var command = new Command<TestParameters, CommandResponse<string>>(new TestParameters(nameof(MessageSenderCallsCorrectHandlerForCommandWithResponse)), Constants.CorrelationId);

        // Act
        var response = await Subject.Send(command, CancellationToken.None);

        // Assert
        await Verifier.Received(1).Receive(Arg.Is(command.Params.Value));

        response.Payload.Should().NotBeNull();
        response.Payload.Should().Be(nameof(TestCommandWithResponseHandler));
    }

    [Fact]
    public async Task MessageSenderCallsCorrectHandlerForEvent()
    {
        // Arrange
        var @event = new Event<TestParameters>(new TestParameters(nameof(MessageSenderCallsCorrectHandlerForEvent)), Constants.CorrelationId, Constants.CausationId);

        // Act
        await Subject.Publish(@event, CancellationToken.None);

        // Assert
        await Verifier.Received(1).Receive(Arg.Is(@event.Params.Value));
    }

    [Fact]
    public async Task MessageSenderCallsCorrectHandlerForEvents()
    {
        // Arrange
        var @event = new Event<TestParameters>(new TestParameters(nameof(MessageSenderCallsCorrectHandlerForEvents)), Constants.CorrelationId, Constants.CausationId);
        var events = new List<Event<TestParameters>>
        {
            @event
        };

        var enumerableEvents = events.AsEnumerable();

        // Act
        await Subject.Publish(enumerableEvents, CancellationToken.None);

        // Assert
        await Verifier.Received(1).Receive(@event.Params.Value);
    }

    [Fact]
    public async Task MessageSenderCallsCorrectHandlerForQuery()
    {
        // Arrange
        var query = new Query<TestParameters, QueryResponse<string>>(new TestParameters(nameof(MessageSenderCallsCorrectHandlerForQuery)), Constants.CorrelationId);

        // Act
        var response = await Subject.Send(query, CancellationToken.None);

        // Assert
        await Verifier.Received(1).Receive(Arg.Is(query.Params.Value));

        response.Payload.Should().NotBeNull();
        response.Payload.Should().Be(nameof(TestQueryHandler));
    }

    [Fact]
    public async Task MessageSenderThrowsWhenCommandHandlerIsNotFound()
    {
        // Arrange
        var command = new Command<string>(string.Empty, Constants.CorrelationId, Constants.CausationId);
        var typeRef = typeof(ICommandHandler<Command<string>>).AssemblyQualifiedName;

        // Act
        Func<Task> subjectResult = async () => { await Subject.Send(command, CancellationToken.None); };

        // Assert
        await subjectResult.Should().ThrowAsync<HandlerNotFoundException>()
                            .WithMessage($"No concrete handlers for type '{typeRef}' could be found.");
    }

    [Fact]
    public async Task MessageSenderThrowsWhenEventHandlerIsNotFound()
    {
        // Arrange
        var @event = new Event<string>(string.Empty, Constants.CorrelationId, Constants.CausationId);
        var typeRef = typeof(IEventHandler<Event<string>>).AssemblyQualifiedName;

        // Act
        Func<Task> subjectResult = async () => { await Subject.Publish(@event, CancellationToken.None); };

        // Assert
        await subjectResult.Should().ThrowAsync<HandlerNotFoundException>()
                            .WithMessage($"No concrete handlers for type '{typeRef}' could be found.");
    }

    [Fact]
    public async Task MessageSenderThrowsWhenQueryHandlerIsNotFound()
    {
        // Arrange
        var query = new Query<object, QueryResponse<string>>(string.Empty, Constants.CorrelationId);
        var typeRef = typeof(IQueryHandler<Query<object, QueryResponse<string>>, QueryResponse<string>>).AssemblyQualifiedName;

        // Act
        Func<Task> subjectResult = async () => { await Subject.Send(query, CancellationToken.None); };

        // Assert
        await subjectResult.Should().ThrowAsync<HandlerNotFoundException>()
                            .WithMessage($"No concrete handlers for type '{typeRef}' could be found.");
    }
}
