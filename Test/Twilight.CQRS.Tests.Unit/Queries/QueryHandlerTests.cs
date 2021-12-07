using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Twilight.CQRS.Queries;
using Twilight.CQRS.Tests.Unit.Common;
using Xunit;

namespace Twilight.CQRS.Tests.Unit.Queries;

public sealed class QueryHandlerTests
{
    private readonly ILogger<TestQueryHandler> _logger;
    private readonly TestQueryHandler _subject;

    // Setup
    public QueryHandlerTests()
    {
        _logger = Substitute.For<ILogger<TestQueryHandler>>();

        IValidator<Query<TestParameters, QueryResponse<TestQueryResponse>>> validator = new TestQueryParametersValidator();

        _subject = new TestQueryHandler(_logger, validator);
    }

    [Fact]
    public async Task HandlerShouldHandleQuery()
    {
        // Arrange
        var testQuery = new Query<TestParameters, QueryResponse<TestQueryResponse>>(new TestParameters(), Constants.CorrelationId);

        // Act
        var response = await _subject.Handle(testQuery, CancellationToken.None);

        // Assert
        response.Payload.Value.Should().Be("1");
    }

    [Fact]
    public async Task HandlerShouldNotThrowWhenValidatingValidQueryParameters()
    {
        // Arrange
        var testQuery = new Query<TestParameters, QueryResponse<TestQueryResponse>>(new TestParameters(), Constants.CorrelationId);

        // Act
        Func<Task> subjectResult = async () => { await _subject.Handle(testQuery, CancellationToken.None); };

        // Assert
        await subjectResult.Should().NotThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task HandlerShouldThrowWhenValidatingInvalidQueryParameters()
    {
        // Arrange
        var testQuery = new Query<TestParameters, QueryResponse<TestQueryResponse>>(new TestParameters(string.Empty), Constants.CorrelationId);

        // Act
        Func<Task> subjectResult = async () => { await _subject.Handle(testQuery, CancellationToken.None); };

        // Assert
        await subjectResult.Should().ThrowAsync<ValidationException>()
                           .WithMessage($"Validation failed: {Environment.NewLine} -- Params.Value: 'Params Value' must not be empty. Severity: Error");
    }
}
