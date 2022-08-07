using FluentAssertions;
using Twilight.CQRS.Commands;
using Twilight.CQRS.Tests.Common;
using Xunit;

namespace Twilight.CQRS.Tests.Unit.Commands;

public sealed class CommandTests
{
    private readonly TestParameters _params;

    // Setup
    public CommandTests()
        => _params = new TestParameters();

    [Fact]
    public void CommandWithoutParametersShouldAssignCausationId()
    {
        // arrange / Act
        var subject = new CqrsCommand(Constants.CorrelationId, Constants.CausationId);

        // Assert
        subject.CausationId.Should().Be(Constants.CausationId);
    }

    [Fact]
    public void CommandWithoutParametersShouldAssignCorrelationId()
    {
        // Arrange / Act
        var subject = new CqrsCommand(Constants.CorrelationId, Constants.CausationId);

        //Assert
        subject.CorrelationId.Should().Be(Constants.CorrelationId);
    }

    [Fact]
    public void CommandWithParametersShouldAssignCausationId()
    {
        // Arrange / Act
        var subject = new CqrsCommand<TestParameters>(_params, Constants.CorrelationId, null, Constants.CausationId);

        // Assert
        subject.CausationId.Should().Be(Constants.CausationId);
    }

    [Fact]
    public void CommandWithParametersShouldAssignCorrelationId()
    {
        // Arrange / Act
        var subject = new CqrsCommand<TestParameters>(_params, Constants.CorrelationId, null, Constants.CausationId);

        // Assert
        subject.CorrelationId.Should().Be(Constants.CorrelationId);
    }

    [Fact]
    public void CommandWithParametersShouldAssignMessageId()
    {
        // Arrange / Act
        var subject = new CqrsCommand<TestParameters>(_params, Constants.CorrelationId, null, Constants.CausationId);

        subject.MessageId.Should().NotBeEmpty();
    }

    [Fact]
    public void CommandWithParametersShouldAssignParameters()
    {
        // Arrange / Act
        var subject = new CqrsCommand<TestParameters>(_params, Constants.CorrelationId, null, Constants.CausationId);

        // Assert
        subject.Params.Should().NotBeNull();
        subject.Params.Should().BeEquivalentTo(_params);
    }

    [Fact]
    public void CommandWithParametersAssignsSessionId()
    {
        // Arrange / Act
        var subject = new CqrsCommand<TestParameters>(_params, Constants.CorrelationId, Constants.SessionId, Constants.CausationId);

        //Assert
        subject.SessionId.Should().Be(Constants.SessionId);
    }
    [Fact]
    public void CommandWithoutParametersAssignsSessionId()
    {
        // Arrange / Act
        var subject = new CqrsCommand(Constants.CorrelationId, Constants.CausationId, Constants.SessionId);

        //Assert
        subject.SessionId.Should().Be(Constants.SessionId);
    }
}
