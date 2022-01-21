using FluentAssertions;
using Twilight.CQRS.Commands;
using Twilight.CQRS.Events;
using Twilight.CQRS.Interfaces;
using Twilight.CQRS.Messaging.Common;
using Twilight.CQRS.Messaging.InMemory.Autofac.Tests.Integration.Setup;
using Twilight.CQRS.Messaging.InMemory.Autofac.Tests.Integration.Setup.Parameters;
using Twilight.CQRS.Tests.Common;
using Xunit;

namespace Twilight.CQRS.Messaging.InMemory.Autofac.Tests.Integration;

public sealed class AutofacInMemoryMessageSenderExceptionTests : IntegrationTestBase
{
    [Fact]
    public async Task MessageSenderThrowsWhenCommandHandlerDoesNotExist()
    {
        // Arrange
        var command = new CqrsCommand(Constants.CorrelationId);

        // Act
        Func<Task> subjectResult = async () => { await Subject.Send(command, CancellationToken.None); };

        // Assert
        await subjectResult.Should().ThrowAsync<HandlerNotFoundException>()
                           .WithMessage($"No concrete handlers for type '{typeof(ICqrsCommandHandler<CqrsCommand>).AssemblyQualifiedName}' could be found.");
    }

    [Fact]
    public async Task MessageSenderThrowsWhenMultipleCommandHandlersResolved()
    {
        // Arrange
        var parameters = new MultipleHandlersParameters();
        var command = new CqrsCommand<MultipleHandlersParameters>(parameters, Constants.CorrelationId);

        // Act
        Func<Task> subjectResult = async () => { await Subject.Send(command, CancellationToken.None); };

        // Assert
        await subjectResult.Should().ThrowAsync<MultipleCommandHandlersDefinedException>()
                           .WithMessage($"Multiple command handlers for type '{typeof(ICqrsCommandHandler<CqrsCommand<MultipleHandlersParameters>>).AssemblyQualifiedName}' were found. Only one handler can exist for a command.");
    }

    [Fact]
    public async Task MessageSenderThrowsWhenNoHandlerRegisteredForEvent()
    {
        // Arrange
        var @event = new CqrsEvent<string>(string.Empty, Constants.CorrelationId, Constants.CausationId);

        // Act
        Func<Task> subjectResult = async () => { await Subject.Publish(@event, CancellationToken.None); };

        // Assert
        await subjectResult.Should().ThrowAsync<HandlerNotFoundException>()
                           .WithMessage($"No concrete handlers for type '{typeof(ICqrsEventHandler<CqrsEvent<string>>).AssemblyQualifiedName}' could be found.");
    }
}
