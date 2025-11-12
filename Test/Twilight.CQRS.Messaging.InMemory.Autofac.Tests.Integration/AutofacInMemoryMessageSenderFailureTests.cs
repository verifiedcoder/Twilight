using Twilight.CQRS.Commands;
using Twilight.CQRS.Events;
using Twilight.CQRS.Messaging.InMemory.Autofac.Tests.Integration.Setup;
using Twilight.CQRS.Tests.Common;

namespace Twilight.CQRS.Messaging.InMemory.Autofac.Tests.Integration;

public sealed class AutofacInMemoryMessageSenderFailureTests : IntegrationTestBase
{
    [Fact]
    public async Task MessageSender_ThrowsWhenCommandHandlerDoesNotExist()
    {
        // Arrange
        var command = new CqrsCommand(Constants.CorrelationId);

        // Act
        var result = await Subject.Send(command, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.Errors[0].Message.ShouldBe("No handler could be found for this request.");
    }

    [Fact]
    public async Task MessageSender_ThrowsWhenMultipleCommandHandlersResolved()
    {
        // Arrange
        var parameters = new MultipleHandlersParameters();
        var command = new CqrsCommand<MultipleHandlersParameters>(parameters, Constants.CorrelationId);

        // Act
        var result = await Subject.Send(command, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.Errors[0].Message.ShouldBe("Multiple handlers found. A command may only have one handler.");
    }

    [Fact]
    public async Task MessageSender_ThrowsWhenNoHandlerRegisteredForEvent()
    {
        // Arrange
        var @event = new CqrsEvent<string>(string.Empty, Constants.CorrelationId, Constants.CausationId);

        // Act
        var result = await Subject.Publish(@event, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.Errors[0].Message.ShouldBe("No handler could be found for this request.");
    }
}
