using FluentAssertions;
using Twilight.CQRS.Commands;
using Twilight.CQRS.Tests.Unit.Shared;
using Xunit;

namespace Twilight.CQRS.Tests.Unit.Commands
{
    public sealed class CommandTests
    {
        private readonly TestParameters _params;
        public CommandTests() => _params = new TestParameters();

        [Fact]
        public void EventWithoutParametersShouldAssignCausationIdWhenCreated()
        {
            var subject = new Command(Constants.CorrelationId, Constants.CausationId);

            subject.CausationId.Should().Be(Constants.CausationId);
        }

        [Fact]
        public void EventWithoutParametersShouldAssignCorrelationIdWhenCreated()
        {
            var subject = new Command(Constants.CorrelationId, Constants.CausationId);

            subject.CorrelationId.Should().Be(Constants.CorrelationId);
        }

        [Fact]
        public void EventWithParametersShouldAssignCausationIdWhenCreated()
        {
            var subject = new Command<TestParameters>(_params, Constants.CorrelationId, Constants.CausationId);

            subject.CausationId.Should().Be(Constants.CausationId);
        }

        [Fact]
        public void EventWithParametersShouldAssignCorrelationIdWhenCreated()
        {
            var subject = new Command<TestParameters>(_params, Constants.CorrelationId, Constants.CausationId);

            subject.CorrelationId.Should().Be(Constants.CorrelationId);
        }

        [Fact]
        public void EventWithParametersShouldAssignMessageIdWhenCreated()
        {
            var subject = new Command<TestParameters>(_params, Constants.CorrelationId, Constants.CausationId);

            subject.MessageId.Should().NotBeEmpty();
        }

        [Fact]
        public void EventWithParametersShouldAssignParameters()
        {
            var subject = new Command<TestParameters>(_params, Constants.CorrelationId, Constants.CausationId);

            subject.Params.Should().NotBeNull();
            subject.Params.Should().BeEquivalentTo(_params);
        }
    }
}
