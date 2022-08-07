using FluentAssertions;
using Twilight.CQRS.Queries;
using Twilight.CQRS.Tests.Common;
using Xunit;

namespace Twilight.CQRS.Tests.Unit.Queries;

public sealed class QueryTests
{
    private readonly TestParameters _params;

    // Setup
    public QueryTests()
        => _params = new TestParameters();

    [Fact]
    public void QueryWithoutParametersShouldAssignCausationId()
    {
        // Arrange / Act
        var subject = new CqrsQuery<TestQueryResponse>(Constants.CorrelationId, null, Constants.CausationId);

        // Assert
        subject.CausationId.Should().Be(Constants.CausationId);
    }

    [Fact]
    public void QueryWithoutParametersShouldAssignCorrelationId()
    {
        // Arrange / Act
        var subject = new CqrsQuery<TestQueryResponse>(Constants.CorrelationId, Constants.CausationId);

        // Assert
        subject.CorrelationId.Should().Be(Constants.CorrelationId);
    }

    [Fact]
    public void QueryWithParametersShouldAssignCausationId()
    {
        // Arrange / Act
        var subject = new CqrsQuery<TestParameters, TestQueryResponse>(_params, Constants.CorrelationId, null, Constants.CausationId);

        // Assert
        subject.CausationId.Should().Be(Constants.CausationId);
    }

    [Fact]
    public void QueryWithParametersShouldAssignCorrelationId()
    {
        // Arrange / Act
        var subject = new CqrsQuery<TestParameters, TestQueryResponse>(_params, Constants.CorrelationId, null, Constants.CausationId);

        // Assert
        subject.CorrelationId.Should().Be(Constants.CorrelationId);
    }

    [Fact]
    public void QueryWithParametersShouldAssignMessageId()
    {
        // Arrange / Act
        var subject = new CqrsQuery<TestParameters, TestQueryResponse>(_params, Constants.CorrelationId, null, Constants.CausationId);

        // Assert
        subject.MessageId.Should().NotBeEmpty();
    }

    [Fact]
    public void QueryWithParametersShouldAssignParameters()
    {
        // Arrange / Act
        var subject = new CqrsQuery<TestParameters, TestQueryResponse>(_params, Constants.CorrelationId, null, Constants.CausationId);

        // Assert
        subject.Params.Should().NotBeNull();
        subject.Params.Should().BeEquivalentTo(_params);
    }

    [Fact]
    public void QueryWithParametersShouldAssignSessionId()
    {
        // Arrange / Act
        var subject = new CqrsQuery<TestParameters, TestQueryResponse>(_params, Constants.CorrelationId, Constants.SessionId, Constants.CausationId);

        // Assert
        subject.SessionId.Should().Be(Constants.SessionId);
    }

    [Fact]
    public void QueryWithoutParametersShouldAssignSessionId()
    {
        // Arrange / Act
        var subject = new CqrsQuery<TestQueryResponse>(Constants.CorrelationId, Constants.SessionId, Constants.CausationId);

        // Assert
        subject.SessionId.Should().Be(Constants.SessionId);
    }
}
