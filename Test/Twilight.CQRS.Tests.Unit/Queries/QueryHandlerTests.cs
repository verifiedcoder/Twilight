using Microsoft.Extensions.Logging;
using Twilight.CQRS.Queries;
using Twilight.CQRS.Tests.Common;

namespace Twilight.CQRS.Tests.Unit.Queries;

public sealed class QueryHandlerTests
{
    private readonly TestCqrsQueryHandler _subject;

    // Setup
    public QueryHandlerTests()
    {
        var logger = Substitute.For<ILogger<TestCqrsQueryHandler>>();

        IValidator<CqrsQuery<TestParameters, QueryResponse<TestQueryResponse>>> validator = new TestQueryParametersValidator();

        _subject = new TestCqrsQueryHandler(logger, validator);
    }

    [Fact]
    public async Task Handler_ShouldHandleQuery()
    {
        // Arrange
        var testQuery = new CqrsQuery<TestParameters, QueryResponse<TestQueryResponse>>(new TestParameters(), Constants.CorrelationId);

        // Act
        var response = await _subject.Handle(testQuery, CancellationToken.None);

        // Assert
        response.Value.Payload.Value.ShouldBe("1");
    }

    [Fact]
    public async Task Handler_ShouldNotThrow_WhenValidatingValidQueryParameters()
    {
        // Arrange
        var testQuery = new CqrsQuery<TestParameters, QueryResponse<TestQueryResponse>>(new TestParameters(), Constants.CorrelationId);

        // Act
        var subjectResult = async () => { await _subject.Handle(testQuery, CancellationToken.None); };

        // Assert
        await subjectResult.ShouldNotThrowAsync();
    }

    [Fact]
    public async Task Handler_ShouldReturnFailedResult_WhenValidatingInvalidQueryParameters()
    {
        // Arrange
        var testQuery = new CqrsQuery<TestParameters, QueryResponse<TestQueryResponse>>(new TestParameters(string.Empty), Constants.CorrelationId);

        // Act
        var result = await _subject.Handle(testQuery, CancellationToken.None);

        // Assert
        result.IsFailed.ShouldBeTrue();
        result.Errors.ShouldContain(error => error.Message.Contains("'Params Value' must not be empty"));
    }
}
