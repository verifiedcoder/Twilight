using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Twilight.CQRS.Queries;
using Twilight.CQRS.Tests.Unit.Shared;
using Xunit;

namespace Twilight.CQRS.Tests.Unit.Queries
{
    public sealed class NonValidatingQueryHandlerTests
    {
        public NonValidatingQueryHandlerTests()
        {
            var logger = Substitute.For<ILogger<NonValidatingTestQueryHandler>>();

            logger.IsEnabled(LogLevel.Trace).Returns(true);

            _subject = new NonValidatingTestQueryHandler(logger);
        }

        private readonly NonValidatingTestQueryHandler _subject;

        [Fact]
        public async Task HandlerShouldNotThrowWhenEventValidatorIsDefault()
        {
            var testQuery = new Query<string, QueryResponse<string>>(string.Empty, Constants.CorrelationId);

            Func<Task> subjectResult = async () => { await _subject.Handle(testQuery, CancellationToken.None); };

            await subjectResult.Should().NotThrowAsync<ValidationException>();
        }
    }
}
