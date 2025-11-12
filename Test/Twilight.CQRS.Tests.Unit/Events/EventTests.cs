using Twilight.CQRS.Events;
using Twilight.CQRS.Tests.Common;

namespace Twilight.CQRS.Tests.Unit.Events;

public sealed class EventTests
{
    private readonly TestParameters _params = new();

    [Fact]
    public void Event_WithoutParameters_ShouldAssignCausationId()
    {
        // Arrange / Act
        var subject = new CqrsEvent(Constants.CorrelationId, null, Constants.CausationId);

        // Assert
        subject.CausationId.ShouldBe(Constants.CausationId);
    }

    [Fact]
    public void Event_WithoutParameters_ShouldAssignCorrelationId()
    {
        // Arrange / Act
        var subject = new CqrsEvent(Constants.CorrelationId, null, Constants.CausationId);

        // Assert
        subject.CorrelationId.ShouldBe(Constants.CorrelationId);
    }

    [Fact]
    public void Event_WithParameters_ShouldAssignCausationId()
    {
        // Arrange / Act
        var subject = new CqrsEvent<TestParameters>(_params, Constants.CorrelationId, null, Constants.CausationId);

        // Assert
        subject.CausationId.ShouldBe(Constants.CausationId);
    }

    [Fact]
    public void Event_WithParameters_ShouldAssignCorrelationId()
    {
        // Arrange / Act
        var subject = new CqrsEvent<TestParameters>(_params, Constants.CorrelationId, null, Constants.CausationId);

        // Assert
        subject.CorrelationId.ShouldBe(Constants.CorrelationId);
    }

    [Fact]
    public void Event_WithParameters_ShouldAssignMessageId()
    {
        // Arrange / Act
        var subject = new CqrsEvent<TestParameters>(_params, Constants.CorrelationId, null, Constants.CausationId);

        // Assert
        subject.MessageId.ShouldNotBeEmpty();
    }

    [Fact]
    public void Event_WithParameters_ShouldAssignParameters()
    {
        // Arrange / Act
        var subject = new CqrsEvent<TestParameters>(_params, Constants.CorrelationId, null, Constants.CausationId);

        // Assert
        subject.Params.ShouldNotBeNull();
        subject.Params.ShouldBeEquivalentTo(_params);
    }

    [Fact]
    public void Query_WithParameters_AssignsSessionId()
    {
        // Arrange / Act
        var subject = new CqrsEvent<TestParameters>(_params, Constants.CorrelationId, Constants.SessionId, Constants.CausationId);

        //Assert
        subject.SessionId.ShouldBe(Constants.SessionId);
    }

    [Fact]
    public void Query_WithoutParameters_AssignsSessionId()
    {
        // Arrange / Act
        var subject = new CqrsEvent(Constants.CorrelationId, Constants.SessionId, Constants.CausationId);

        //Assert
        subject.SessionId.ShouldBe(Constants.SessionId);
    }
}
