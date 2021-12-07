using FluentAssertions;
using Twilight.CQRS.Events;
using Twilight.CQRS.Tests.Unit.Common;
using Xunit;

namespace Twilight.CQRS.Tests.Unit.Events;

public sealed class EventTests
{
    private readonly TestParameters _params;

    // Setup
    public EventTests()
        => _params = new TestParameters();

    [Fact]
    public void EventWithoutParametersShouldAssignCausationIdWhenCreated()
    {
        // Arrange / Act
        var subject = new Event(Constants.CorrelationId, Constants.CausationId);

        // Assert
        subject.CausationId.Should().Be(Constants.CausationId);
    }

    [Fact]
    public void EventWithoutParametersShouldAssignCorrelationIdWhenCreated()
    {
        // Arrange / Act
        var subject = new Event(Constants.CorrelationId, Constants.CausationId);

        // Assert
        subject.CorrelationId.Should().Be(Constants.CorrelationId);
    }

    [Fact]
    public void EventWithParametersShouldAssignCausationIdWhenCreated()
    {
        // Arrange / Act
        var subject = new Event<TestParameters>(_params, Constants.CorrelationId, Constants.CausationId);

        // Assert
        subject.CausationId.Should().Be(Constants.CausationId);
    }

    [Fact]
    public void EventWithParametersShouldAssignCorrelationIdWhenCreated()
    {
        // Arrange / Act
        var subject = new Event<TestParameters>(_params, Constants.CorrelationId, Constants.CausationId);

        // Assert
        subject.CorrelationId.Should().Be(Constants.CorrelationId);
    }

    [Fact]
    public void EventWithParametersShouldAssignMessageIdWhenCreated()
    {
        // Arrange / Act
        var subject = new Event<TestParameters>(_params, Constants.CorrelationId, Constants.CausationId);

        // Assert
        subject.MessageId.Should().NotBeEmpty();
    }

    [Fact]
    public void EventWithParametersShouldAssignParameters()
    {
        // Arrange / Act
        var subject = new Event<TestParameters>(_params, Constants.CorrelationId, Constants.CausationId);

        // Assert
        subject.Params.Should().NotBeNull();
        subject.Params.Should().BeEquivalentTo(_params);
    }
}
