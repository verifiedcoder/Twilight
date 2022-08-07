using FluentAssertions;
using Twilight.CQRS.Events;
using Twilight.CQRS.Tests.Common;
using Xunit;

namespace Twilight.CQRS.Tests.Unit.Events;

public sealed class EventTests
{
    private readonly TestParameters _params;

    // Setup
    public EventTests()
        => _params = new TestParameters();

    [Fact]
    public void EventWithoutParametersShouldAssignCausationId()
    {
        // Arrange / Act
        var subject = new CqrsEvent(Constants.CorrelationId, null, Constants.CausationId);

        // Assert
        subject.CausationId.Should().Be(Constants.CausationId);
    }

    [Fact]
    public void EventWithoutParametersShouldAssignCorrelationId()
    {
        // Arrange / Act
        var subject = new CqrsEvent(Constants.CorrelationId, null, Constants.CausationId);

        // Assert
        subject.CorrelationId.Should().Be(Constants.CorrelationId);
    }

    [Fact]
    public void EventWithParametersShouldAssignCausationId()
    {
        // Arrange / Act
        var subject = new CqrsEvent<TestParameters>(_params, Constants.CorrelationId, null, Constants.CausationId);

        // Assert
        subject.CausationId.Should().Be(Constants.CausationId);
    }

    [Fact]
    public void EventWithParametersShouldAssignCorrelationId()
    {
        // Arrange / Act
        var subject = new CqrsEvent<TestParameters>(_params, Constants.CorrelationId, null, Constants.CausationId);

        // Assert
        subject.CorrelationId.Should().Be(Constants.CorrelationId);
    }

    [Fact]
    public void EventWithParametersShouldAssignMessageId()
    {
        // Arrange / Act
        var subject = new CqrsEvent<TestParameters>(_params, Constants.CorrelationId, null, Constants.CausationId);

        // Assert
        subject.MessageId.Should().NotBeEmpty();
    }

    [Fact]
    public void EventWithParametersShouldAssignParameters()
    {
        // Arrange / Act
        var subject = new CqrsEvent<TestParameters>(_params, Constants.CorrelationId, null, Constants.CausationId);

        // Assert
        subject.Params.Should().NotBeNull();
        subject.Params.Should().BeEquivalentTo(_params);
    }

    [Fact]
    public void QueryWithParametersAssignsSessionId()
    {
        // Arrange / Act
        var subject = new CqrsEvent<TestParameters>(_params, Constants.CorrelationId, Constants.SessionId, Constants.CausationId);

        //Assert
        subject.SessionId.Should().Be(Constants.SessionId);
    }

    [Fact]
    public void QueryWithoutParametersAssignsSessionId()
    {
        // Arrange / Act
        var subject = new CqrsEvent(Constants.CorrelationId, Constants.SessionId, Constants.CausationId);

        //Assert
        subject.SessionId.Should().Be(Constants.SessionId);
    }
}
