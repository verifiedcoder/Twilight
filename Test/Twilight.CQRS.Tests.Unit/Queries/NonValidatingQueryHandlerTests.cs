using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using FluentValidation;
using Twilight.CQRS.Queries;
using Twilight.CQRS.Tests.Unit.Shared;
using Xunit;

namespace Twilight.CQRS.Tests.Unit.Queries
{
    public sealed class NonValidatingQueryHandlerTests
    {
        private readonly NonValidatingTestQueryHandler _subject;

        public NonValidatingQueryHandlerTests() => _subject = new NonValidatingTestQueryHandler();

        [Fact]
        public async Task HandlerShouldNotThrowWhenEventValidatorIsDefault()
        {
            var testQuery = new Query<string, QueryResponse<string>>(string.Empty, Constants.CorrelationId);

            Func<Task> subjectResult = async () => { await _subject.Handle(testQuery, CancellationToken.None); };

            await subjectResult.Should().NotThrowAsync<ValidationException>();
        }
    }
}
