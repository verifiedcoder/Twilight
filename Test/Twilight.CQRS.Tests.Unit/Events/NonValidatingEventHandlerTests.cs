using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Twilight.CQRS.Events;
using Twilight.CQRS.Tests.Unit.Shared;
using Xunit;

namespace Twilight.CQRS.Tests.Unit.Events
{
    public sealed class NonValidatingEventHandlerTests
    {
        public NonValidatingEventHandlerTests()
        {
            var logger = Substitute.For<ILogger<NonValidatingTestEventHandler>>();

            logger.IsEnabled(LogLevel.Trace).Returns(true);

            _subject = new NonValidatingTestEventHandler(logger);
        }

        private readonly NonValidatingTestEventHandler _subject;

        [Fact]
        public async Task HandlerShouldNotThrowWhenEventValidatorIsDefault()
        {
            var testEvent = new Event<string>(string.Empty, Constants.CorrelationId);

            Func<Task> subjectResult = async () => { await _subject.Handle(testEvent, CancellationToken.None); };

            await subjectResult.Should().NotThrowAsync<ValidationException>();
        }
    }
}
