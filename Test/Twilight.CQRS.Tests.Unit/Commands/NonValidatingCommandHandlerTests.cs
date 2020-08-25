using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Twilight.CQRS.Commands;
using Twilight.CQRS.Messaging.Contracts;
using Twilight.CQRS.Tests.Unit.Shared;
using Xunit;

namespace Twilight.CQRS.Tests.Unit.Commands
{
    public sealed class NonValidatingCommandHandlerTests
    {
        public NonValidatingCommandHandlerTests()
        {
            var logger = Substitute.For<ILogger<NonValidatingTestCommandHandler>>();

            logger.IsEnabled(LogLevel.Trace).Returns(true);

            var messageSender = Substitute.For<IMessageSender>();

            _subject = new NonValidatingTestCommandHandler(messageSender, logger);
        }

        private readonly NonValidatingTestCommandHandler _subject;

        [Fact]
        public async Task HandlerShouldNotThrowWhenCommandValidatorIsDefault()
        {
            var testCommand = new Command<string>(string.Empty, Constants.CorrelationId);

            Func<Task> subjectResult = async () => { await _subject.Handle(testCommand, CancellationToken.None); };

            await subjectResult.Should().NotThrowAsync<ValidationException>();
        }
    }
}
