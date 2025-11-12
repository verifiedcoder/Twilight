using Twilight.CQRS.Commands;
using Twilight.CQRS.Tests.Common;

namespace Twilight.CQRS.Tests.Unit.Commands;

public sealed class CommandTests
{
    private readonly TestParameters _params = new();

    [Fact]
    public void Command_WithoutParameters_ShouldAssignCausationId()
    {
        // arrange / Act
        var subject = new CqrsCommand(Constants.CorrelationId, Constants.CausationId);

        // Assert
        subject.CausationId.ShouldBe(Constants.CausationId);
    }

    [Fact]
    public void Command_WithoutParameters_ShouldAssignCorrelationId()
    {
        // Arrange / Act
        var subject = new CqrsCommand(Constants.CorrelationId, Constants.CausationId);

        //Assert
        subject.CorrelationId.ShouldBe(Constants.CorrelationId);
    }

    [Fact]
    public void Command_WithParameters_ShouldAssignCausationId()
    {
        // Arrange / Act
        var subject = new CqrsCommand<TestParameters>(_params, Constants.CorrelationId, null, Constants.CausationId);

        // Assert
        subject.CausationId.ShouldBe(Constants.CausationId);
    }

    [Fact]
    public void Command_WithParameters_ShouldAssignCorrelationId()
    {
        // Arrange / Act
        var subject = new CqrsCommand<TestParameters>(_params, Constants.CorrelationId, null, Constants.CausationId);

        // Assert
        subject.CorrelationId.ShouldBe(Constants.CorrelationId);
    }

    [Fact]
    public void Command_WithParameters_ShouldAssignMessageId()
    {
        // Arrange / Act
        var subject = new CqrsCommand<TestParameters>(_params, Constants.CorrelationId, null, Constants.CausationId);

        subject.MessageId.ShouldNotBeEmpty();
    }

    [Fact]
    public void Command_WithParameters_ShouldAssignParameters()
    {
        // Arrange / Act
        var subject = new CqrsCommand<TestParameters>(_params, Constants.CorrelationId, null, Constants.CausationId);

        // Assert
        subject.Params.ShouldNotBeNull();
        subject.Params.ShouldBeEquivalentTo(_params);
    }

    [Fact]
    public void Command_WithParameters_AssignsSessionId()
    {
        // Arrange / Act
        var subject = new CqrsCommand<TestParameters>(_params, Constants.CorrelationId, Constants.SessionId, Constants.CausationId);

        //Assert
        subject.SessionId.ShouldBe(Constants.SessionId);
    }
    
    [Fact]
    public void Command_WithoutParameters_AssignsSessionId()
    {
        // Arrange / Act
        var subject = new CqrsCommand(Constants.CorrelationId, Constants.CausationId, Constants.SessionId);

        //Assert
        subject.SessionId.ShouldBe(Constants.SessionId);
    }
}
