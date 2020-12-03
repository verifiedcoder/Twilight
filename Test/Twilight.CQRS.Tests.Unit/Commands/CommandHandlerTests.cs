using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Twilight.CQRS.Commands;
using Twilight.CQRS.Events;
using Twilight.CQRS.Messaging.Contracts;
using Twilight.CQRS.Tests.Unit.Shared;
using Xunit;

namespace Twilight.CQRS.Tests.Unit.Commands
{
    public sealed class CommandHandlerTests
    {
        private readonly IMessageSender _messageSender;
        private readonly TestCommandHandler _subject;

        public CommandHandlerTests()
        {
            var logger = Substitute.For<ILogger<TestCommandHandler>>();

            logger.IsEnabled(LogLevel.Trace).Returns(true);

            _messageSender = Substitute.For<IMessageSender>();

            IValidator<Command<TestParameters>> validator = new TestParametersValidator();

            _subject = new TestCommandHandler(_messageSender, logger, validator);
        }

        [Fact]
        public async Task HandlerShouldPublishEventWhenHandling()
        {
            var testCommand = new Command<TestParameters>(new TestParameters(), Constants.CorrelationId);

            await _subject.Handle(testCommand, CancellationToken.None);

            await _messageSender.Received(1).Publish(Arg.Any<Event<TestParameters>>(), Arg.Is(CancellationToken.None));
        }

        [Fact]
        public async Task HandlerShouldThrowWhenValidatingInvalidCommandParameters()
        {
            var testCommand = new Command<TestParameters>(new TestParameters(string.Empty), Constants.CorrelationId);

            Func<Task> subjectResult = async () => { await _subject.Handle(testCommand, CancellationToken.None); };

            await subjectResult.Should().ThrowAsync<ValidationException>()
                               .WithMessage($"Validation failed: {Environment.NewLine} -- Params.Value: 'Params. Value' must not be empty.");
        }

        [Fact]
        public async Task HandlerShouldValidateValidCommandParameters()
        {
            var testCommand = new Command<TestParameters>(new TestParameters(), Constants.CorrelationId);

            Func<Task> subjectResult = async () => { await _subject.Handle(testCommand, CancellationToken.None); };

            await subjectResult.Should().NotThrowAsync<ValidationException>();
        }
    }
}
