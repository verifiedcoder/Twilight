using FluentValidation;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Twilight.CQRS.Commands;
using Twilight.CQRS.Events;
using Twilight.CQRS.Messaging.Interfaces;
using Twilight.CQRS.Tests.Common;
using Xunit;

namespace Twilight.CQRS.Tests.Unit.Commands;

public sealed class CommandHandlerTests
{
    private readonly IMessageSender _messageSender;
    private readonly TestCqrsCommandHandler _subject;

    public CommandHandlerTests()
    {
        // Setup
        _messageSender = Substitute.For<IMessageSender>();

        var logger = Substitute.For<ILogger<TestCqrsCommandHandler>>();

        IValidator<CqrsCommand<TestParameters>> validator = new TestParametersValidator();

        _subject = new TestCqrsCommandHandler(_messageSender, logger, validator);
    }

    [Fact]
    public async Task HandlerShouldPublishEventWhenHandling()
    {
        // Arrange
        var testCommand = new CqrsCommand<TestParameters>(new TestParameters(), Constants.CorrelationId);

        // Act
        await _subject.HandleCommand(testCommand, CancellationToken.None);

        // Assert
        await _messageSender.Received(1).Publish(Arg.Any<CqrsEvent<TestParameters>>(), Arg.Is(CancellationToken.None));
    }
}
