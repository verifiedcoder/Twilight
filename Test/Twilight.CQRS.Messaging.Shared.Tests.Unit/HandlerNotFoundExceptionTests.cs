using System;
using FluentAssertions;
using Xunit;

namespace Twilight.CQRS.Messaging.Shared.Tests.Unit
{
    public class HandlerNotFoundExceptionTests
    {
        private readonly HandlerNotFoundException _subject;
        public HandlerNotFoundExceptionTests() => _subject = new HandlerNotFoundException(typeof(Guid).AssemblyQualifiedName ?? string.Empty);

        [Fact]
        public void ExceptionShouldConstructCorrectMessage()
        {
            _subject.Message.Should()
                    .Be("No concrete handlers for type 'System.Guid, System.Private.CoreLib, Version=5.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e' could be found.");
        }

        [Fact]
        public void SubjectShouldDeriveFromException()
        {
            _subject.GetBaseException().Should().BeAssignableTo<Exception>();
        }
    }
}
