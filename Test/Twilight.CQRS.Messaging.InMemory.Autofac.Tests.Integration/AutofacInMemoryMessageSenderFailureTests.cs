using FluentAssertions;
using Twilight.CQRS.Commands;
using Twilight.CQRS.Events;
using Twilight.CQRS.Messaging.InMemory.Autofac.Tests.Integration.Setup;
using Twilight.CQRS.Messaging.InMemory.Autofac.Tests.Integration.Setup.Parameters;
using Twilight.CQRS.Tests.Common;
using Xunit;

namespace Twilight.CQRS.Messaging.InMemory.Autofac.Tests.Integration;

public sealed class AutofacInMemoryMessageSenderFailureTests : IntegrationTestBase
{
    [Fact]
    public async Task MessageSenderThrowsWhenCommandHandlerDoesNotExist()
    {
        // Arrange
        var command = new CqrsCommand(Constants.CorrelationId);

        // Act
        var result = await Subject.Send(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Message.Should().Be("No handler cold be found for this request.");
    }

    [Fact]
    public async Task MessageSenderThrowsWhenMultipleCommandHandlersResolved()
    {
        // Arrange
        var parameters = new MultipleHandlersParameters();
        var command = new CqrsCommand<MultipleHandlersParameters>(parameters, Constants.CorrelationId);

        // Act
        var result = await Subject.Send(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Message.Should().Be("Multiple handlers found. A command may only have one handler.");
    }

    [Fact]
    public async Task MessageSenderThrowsWhenNoHandlerRegisteredForEvent()
    {
        // Arrange
        var @event = new CqrsEvent<string>(string.Empty, Constants.CorrelationId, Constants.CausationId);

        // Act
        var result = await Subject.Publish(@event, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Message.Should().Be("No handler cold be found for this request.");
    }
}
