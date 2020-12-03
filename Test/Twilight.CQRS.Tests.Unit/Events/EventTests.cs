using FluentAssertions;
using Twilight.CQRS.Events;
using Twilight.CQRS.Tests.Unit.Shared;
using Xunit;

namespace Twilight.CQRS.Tests.Unit.Events
{
    public sealed class EventTests
    {
        private readonly TestParameters _params;
        public EventTests() => _params = new TestParameters();

        [Fact]
        public void EventWithoutParametersShouldAssignCausationIdWhenCreated()
        {
            var subject = new Event(Constants.CorrelationId, Constants.CausationId);

            subject.CausationId.ToString().Should().Be(Constants.CausationId.ToString());
        }

        [Fact]
        public void EventWithoutParametersShouldAssignCorrelationIdWhenCreated()
        {
            var subject = new Event(Constants.CorrelationId, Constants.CausationId);

            subject.CorrelationId.ToString().Should().Be(Constants.CorrelationId.ToString());
        }

        [Fact]
        public void EventWithParametersShouldAssignCausationIdWhenCreated()
        {
            var subject = new Event<TestParameters>(_params, Constants.CorrelationId, Constants.CausationId);

            subject.CausationId.ToString().Should().Be(Constants.CausationId.ToString());
        }

        [Fact]
        public void EventWithParametersShouldAssignCorrelationIdWhenCreated()
        {
            var subject = new Event<TestParameters>(_params, Constants.CorrelationId, Constants.CausationId);

            subject.CorrelationId.ToString().Should().Be(Constants.CorrelationId.ToString());
        }

        [Fact]
        public void EventWithParametersShouldAssignMessageIdWhenCreated()
        {
            var subject = new Event<TestParameters>(_params, Constants.CorrelationId, Constants.CausationId);

            subject.MessageId.Should().NotBeEmpty();
        }

        [Fact]
        public void EventWithParametersShouldAssignParameters()
        {
            var subject = new Event<TestParameters>(_params, Constants.CorrelationId, Constants.CausationId);

            subject.Params.Should().NotBeNull();
            subject.Params.Should().BeEquivalentTo(_params);
        }
    }
}
