using FluentAssertions;
using Xunit;

namespace Twilight.CQRS.Messaging.Common.Tests.Unit;

public class HandlerNotFoundExceptionTests
{
    private readonly HandlerNotFoundException _subject = new(typeof(Guid).AssemblyQualifiedName ?? string.Empty);

    // Setup

    [Fact]
    public void ExceptionShouldConstructCorrectMessage()
        => _subject.Message.Should()
                   .Be("No concrete handlers for type 'System.Guid, System.Private.CoreLib, Version=8.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e' could be found.");

    [Fact]
    public void SubjectShouldDeriveFromException()
        => _subject.GetBaseException().Should().BeAssignableTo<Exception>();
}
