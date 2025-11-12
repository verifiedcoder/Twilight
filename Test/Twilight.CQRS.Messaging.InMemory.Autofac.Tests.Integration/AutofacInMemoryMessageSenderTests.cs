using Twilight.CQRS.Commands;
using Twilight.CQRS.Events;
using Twilight.CQRS.Messaging.InMemory.Autofac.Tests.Integration.Setup;
using Twilight.CQRS.Queries;
using Twilight.CQRS.Tests.Common;

namespace Twilight.CQRS.Messaging.InMemory.Autofac.Tests.Integration;

public sealed class AutofacInMemoryMessageSenderTests : IntegrationTestBase
{
    [Fact]
    public async Task MessageSender_CallsCorrectHandler_ForCommand()
    {
        // Arrange
        var command = new CqrsCommand<TestParameters>(new TestParameters(nameof(MessageSender_CallsCorrectHandler_ForCommand)), Constants.CorrelationId);

        // Act
        await Subject.Send(command, CancellationToken.None);

        // Assert
        await Verifier.Received(1).Receive(Arg.Is(command.Params.Value));
    }

    [Fact]
    public async Task MessageSender_CallsCorrectHandler_ForCommand_WithResponse()
    {
        // Arrange
        var command = new CqrsCommand<TestParameters, CqrsCommandResponse<string>>(new TestParameters(nameof(MessageSender_CallsCorrectHandler_ForCommand_WithResponse)), Constants.CorrelationId);

        // Act
        var result = await Subject.Send(command, CancellationToken.None);

        // Assert
        await Verifier.Received(1).Receive(Arg.Is(command.Params.Value));

        result.Value.ShouldNotBeNull();
        result.Value.CorrelationId.ShouldBe(Constants.CorrelationId);
    }

    [Fact]
    public async Task MessageSender_CallsCorrectHandler_ForEvent()
    {
        // Arrange
        var @event = new CqrsEvent<TestParameters>(new TestParameters(nameof(MessageSender_CallsCorrectHandler_ForEvent)), Constants.CorrelationId, Constants.CausationId);

        // Act
        await Subject.Publish(@event, CancellationToken.None);

        // Assert
        await Verifier.Received(1).Receive(Arg.Is(@event.Params.Value));
    }

    [Fact]
    public async Task MessageSender_CallsCorrectHandler_ForEvents()
    {
        // Arrange
        var @event = new CqrsEvent<TestParameters>(new TestParameters(nameof(MessageSender_CallsCorrectHandler_ForEvents)), Constants.CorrelationId, Constants.CausationId);
        var events = new List<CqrsEvent<TestParameters>>
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
    public async Task MessageSender_CallsCorrectHandler_ForQuery()
    {
        // Arrange
        var query = new CqrsQuery<TestParameters, QueryResponse<string>>(new TestParameters(nameof(MessageSender_CallsCorrectHandler_ForQuery)), Constants.CorrelationId);

        // Act
        var result = await Subject.Send(query, CancellationToken.None);

        // Assert
        await Verifier.Received(1).Receive(Arg.Is(query.Params.Value));

        result.Value.ShouldNotBeNull();
        result.Value.CorrelationId.ShouldBe(Constants.CorrelationId);
    }

    [Fact]
    public async Task MessageSender_ThrowsWhenCommandHandlerIsNotFound()
    {
        // Arrange
        var command = new CqrsCommand<string>(string.Empty, Constants.CorrelationId, Constants.CausationId);

        // Act
        var result = await Subject.Send(command, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.Errors[0].Message.ShouldBe("No handler could be found for this request.");
    }

    [Fact]
    public async Task MessageSender_ThrowsWhenEventHandlerIsNotFound()
    {
        // Arrange
        var @event = new CqrsEvent<string>(string.Empty, Constants.CorrelationId, Constants.CausationId);

        // Act
        var result = await Subject.Publish(@event, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.Errors[0].Message.ShouldBe("No handler could be found for this request.");
    }

    [Fact]
    public async Task MessageSender_ThrowsWhenQueryHandlerIsNotFound()
    {
        // Arrange
        var query = new CqrsQuery<object, QueryResponse<string>>(string.Empty, Constants.CorrelationId);

        // Act
        var result = await Subject.Send(query, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.Errors[0].Message.ShouldBe("No handler could be found for this request.");
    }
}
