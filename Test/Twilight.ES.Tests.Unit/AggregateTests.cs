using System;
using FluentAssertions;
using Xunit;

namespace Twilight.ES.Tests.Unit
{
    public sealed class AggregateTests
    {
        [Fact]
        public void AggregateWithIdentifierShouldAssignIdentifier()
        {
            var aggregateId = Guid.NewGuid();
            var subject = new WithIdentityAggregate(aggregateId);

            subject.AggregateId.Should().NotBeEmpty();
            subject.AggregateId.Should().Be(aggregateId);
        }

        [Fact]
        public void AggregateWithoutIdentityShouldAssignIdentifier()
        {
            var subject = new WithoutIdentityAggregate();

            subject.AggregateId.Should().NotBeEmpty();
        }
    }
}
