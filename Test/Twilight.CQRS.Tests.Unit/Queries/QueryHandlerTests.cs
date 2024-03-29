﻿using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Twilight.CQRS.Queries;
using Twilight.CQRS.Tests.Common;
using Xunit;

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
    public async Task HandlerShouldHandleQuery()
    {
        // Arrange
        var testQuery = new CqrsQuery<TestParameters, QueryResponse<TestQueryResponse>>(new TestParameters(), Constants.CorrelationId);

        // Act
        var response = await _subject.Handle(testQuery, CancellationToken.None);

        // Assert
        response.Value.Payload.Value.Should().Be("1");
    }

    [Fact]
    public async Task HandlerShouldNotThrowWhenValidatingValidQueryParameters()
    {
        // Arrange
        var testQuery = new CqrsQuery<TestParameters, QueryResponse<TestQueryResponse>>(new TestParameters(), Constants.CorrelationId);

        // Act
        var subjectResult = async () => { await _subject.Handle(testQuery, CancellationToken.None); };

        // Assert
        await subjectResult.Should().NotThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task HandlerShouldThrowWhenValidatingInvalidQueryParameters()
    {
        // Arrange
        var testQuery = new CqrsQuery<TestParameters, QueryResponse<TestQueryResponse>>(new TestParameters(string.Empty), Constants.CorrelationId);

        // Act
        var result = await _subject.Handle(testQuery, CancellationToken.None);

        // Assert
    }
}
