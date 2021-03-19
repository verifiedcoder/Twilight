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
    public sealed class NonValidatingEventHandlerTests
    {
        private readonly NonValidatingTestEventHandler _subject;

        public NonValidatingEventHandlerTests() => _subject = new NonValidatingTestEventHandler();

        [Fact]
        public async Task HandlerShouldNotThrowWhenEventValidatorIsDefault()
        {
            var testEvent = new Event<string>(string.Empty, Constants.CorrelationId);

            Func<Task> subjectResult = async () => { await _subject.Handle(testEvent, CancellationToken.None); };

            await subjectResult.Should().NotThrowAsync<ValidationException>();
        }
    }
}
