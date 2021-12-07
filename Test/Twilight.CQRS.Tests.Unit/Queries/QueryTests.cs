using FluentAssertions;
using Twilight.CQRS.Queries;
using Twilight.CQRS.Tests.Unit.Common;
using Xunit;

namespace Twilight.CQRS.Tests.Unit.Queries;

public sealed class QueryTests
{
    private readonly TestParameters _params;

    // Setup
    public QueryTests()
        => _params = new TestParameters();

    [Fact]
    public void QueryWithoutParametersShouldAssignCausationIdWhenCreated()
    {
        // Arrange / Act
        var subject = new Query<TestQueryResponse>(Constants.CorrelationId, Constants.CausationId);

        // Assert
        subject.CausationId.Should().Be(Constants.CausationId);
    }

    [Fact]
    public void QueryWithoutParametersShouldAssignCorrelationIdWhenCreated()
    {
        // Arrange / Act
        var subject = new Query<TestQueryResponse>(Constants.CorrelationId, Constants.CausationId);

        // Assert
        subject.CorrelationId.Should().Be(Constants.CorrelationId);
    }

    [Fact]
    public void QueryWithParametersShouldAssignCausationIdWhenCreated()
    {
        // Arrange / Act
        var subject = new Query<TestParameters, TestQueryResponse>(_params, Constants.CorrelationId, Constants.CausationId);

        // Assert
        subject.CausationId.Should().Be(Constants.CausationId);
    }

    [Fact]
    public void QueryWithParametersShouldAssignCorrelationIdWhenCreated()
    {
        // Arrange / Act
        var subject = new Query<TestParameters, TestQueryResponse>(_params, Constants.CorrelationId, Constants.CausationId);

        // Assert
        subject.CorrelationId.Should().Be(Constants.CorrelationId);
    }

    [Fact]
    public void QueryWithParametersShouldAssignMessageIdWhenCreated()
    {
        // Arrange / Act
        var subject = new Query<TestParameters, TestQueryResponse>(_params, Constants.CorrelationId, Constants.CausationId);

        // Assert
        subject.MessageId.Should().NotBeEmpty();
    }

    [Fact]
    public void QueryWithParametersShouldAssignParameters()
    {
        // Arrange / Act
        var subject = new Query<TestParameters, TestQueryResponse>(_params, Constants.CorrelationId, Constants.CausationId);

        // Assert
        subject.Params.Should().NotBeNull();
        subject.Params.Should().BeEquivalentTo(_params);
    }
}

