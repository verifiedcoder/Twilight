using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using FluentValidation;
using Twilight.CQRS.Events;
using Twilight.CQRS.Tests.Unit.Shared;
using Xunit;

namespace Twilight.CQRS.Tests.Unit.Events
{
    public sealed class EventHandlerTests
    {
        private readonly TestEventHandler _subject;

        public EventHandlerTests()
        {
            IValidator<Event<TestParameters>> validator = new TestEventParametersValidator();

            _subject = new TestEventHandler(validator);
        }

        [Fact]
        public async Task HandlerShouldNotThrowWhenValidatingValidEventParameters()
        {
            var testEvent = new Event<TestParameters>(new TestParameters(), Constants.CorrelationId);

            Func<Task> subjectResult = async () => { await _subject.Handle(testEvent, CancellationToken.None); };

            await subjectResult.Should().NotThrowAsync<ValidationException>();
        }

        [Fact]
        public async Task HandlerShouldThrowWhenValidatingInvalidEventParameters()
        {
            var testEvent = new Event<TestParameters>(new TestParameters(string.Empty), Constants.CorrelationId);

            Func<Task> subjectResult = async () => { await _subject.Handle(testEvent, CancellationToken.None); };

            await subjectResult.Should().ThrowAsync<ValidationException>()
                               .WithMessage($"Validation failed: {Environment.NewLine} -- Params.Value: 'Params. Value' must not be empty.");
        }
    }
}
