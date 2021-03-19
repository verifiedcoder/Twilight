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
    public sealed class QueryHandlerTests
    {
        private readonly TestQueryHandler _subject;

        public QueryHandlerTests()
        {
            IValidator<Query<TestParameters, QueryResponse<TestQueryResponse>>> validator = new TestQueryParametersValidator();

            _subject = new TestQueryHandler(validator);
        }

        [Fact]
        public async Task HandlerShouldHandleQuery()
        {
            var testQuery = new Query<TestParameters, QueryResponse<TestQueryResponse>>(new TestParameters(), Constants.CorrelationId);

            var response = await _subject.Handle(testQuery, CancellationToken.None);

            response.Payload.Value.Should().Be("1");
        }

        [Fact]
        public async Task HandlerShouldNotThrowWhenValidatingValidQueryParameters()
        {
            var testQuery = new Query<TestParameters, QueryResponse<TestQueryResponse>>(new TestParameters(), Constants.CorrelationId);

            Func<Task> subjectResult = async () => { await _subject.Handle(testQuery, CancellationToken.None); };

            await subjectResult.Should().NotThrowAsync<ValidationException>();
        }

        [Fact]
        public async Task HandlerShouldThrowWhenValidatingInvalidQueryParameters()
        {
            var testQuery = new Query<TestParameters, QueryResponse<TestQueryResponse>>(new TestParameters(string.Empty), Constants.CorrelationId);

            Func<Task> subjectResult = async () => { await _subject.Handle(testQuery, CancellationToken.None); };

            await subjectResult.Should().ThrowAsync<ValidationException>()
                               .WithMessage($"Validation failed: {Environment.NewLine} -- Params.Value: 'Params. Value' must not be empty.");
        }
    }
}
