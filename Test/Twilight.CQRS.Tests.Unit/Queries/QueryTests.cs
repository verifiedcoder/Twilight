using Twilight.CQRS.Queries;
using Twilight.CQRS.Tests.Common;

namespace Twilight.CQRS.Tests.Unit.Queries;

public sealed class QueryTests
{
    private readonly TestParameters _params = new();

    [Fact]
    public void Query_WithoutParameters_ShouldAssignCausationId()
    {
        // Arrange / Act
        var subject = new CqrsQuery<TestQueryResponse>(Constants.CorrelationId, null, Constants.CausationId);

        // Assert
        subject.CausationId.ShouldBe(Constants.CausationId);
    }

    [Fact]
    public void Query_WithoutParameters_ShouldAssignCorrelationId()
    {
        // Arrange / Act
        var subject = new CqrsQuery<TestQueryResponse>(Constants.CorrelationId, Constants.CausationId);

        // Assert
        subject.CorrelationId.ShouldBe(Constants.CorrelationId);
    }

    [Fact]
    public void Query_WithParameters_ShouldAssignCausationId()
    {
        // Arrange / Act
        var subject = new CqrsQuery<TestParameters, TestQueryResponse>(_params, Constants.CorrelationId, null, Constants.CausationId);

        // Assert
        subject.CausationId.ShouldBe(Constants.CausationId);
    }

    [Fact]
    public void Query_WithParameters_ShouldAssignCorrelationId()
    {
        // Arrange / Act
        var subject = new CqrsQuery<TestParameters, TestQueryResponse>(_params, Constants.CorrelationId, null, Constants.CausationId);

        // Assert
        subject.CorrelationId.ShouldBe(Constants.CorrelationId);
    }

    [Fact]
    public void Query_WithParameters_ShouldAssignMessageId()
    {
        // Arrange / Act
        var subject = new CqrsQuery<TestParameters, TestQueryResponse>(_params, Constants.CorrelationId, null, Constants.CausationId);

        // Assert
        subject.MessageId.ShouldNotBeEmpty();
    }

    [Fact]
    public void Query_WithParameters_ShouldAssignParameters()
    {
        // Arrange / Act
        var subject = new CqrsQuery<TestParameters, TestQueryResponse>(_params, Constants.CorrelationId, null, Constants.CausationId);

        // Assert
        subject.Params.ShouldNotBeNull();
        subject.Params.ShouldBeEquivalentTo(_params);
    }

    [Fact]
    public void Query_WithParameters_ShouldAssignSessionId()
    {
        // Arrange / Act
        var subject = new CqrsQuery<TestParameters, TestQueryResponse>(_params, Constants.CorrelationId, Constants.SessionId, Constants.CausationId);

        // Assert
        subject.SessionId.ShouldBe(Constants.SessionId);
    }

    [Fact]
    public void Query_WithoutParameters_ShouldAssignSessionId()
    {
        // Arrange / Act
        var subject = new CqrsQuery<TestQueryResponse>(Constants.CorrelationId, Constants.SessionId, Constants.CausationId);

        // Assert
        subject.SessionId.ShouldBe(Constants.SessionId);
    }
}
