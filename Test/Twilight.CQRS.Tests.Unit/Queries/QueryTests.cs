using FluentAssertions;
using Twilight.CQRS.Queries;
using Twilight.CQRS.Tests.Unit.Shared;
using Xunit;

namespace Twilight.CQRS.Tests.Unit.Queries
{
    public sealed class QueryTests
    {
        public QueryTests() => _params = new TestParameters();

        private readonly TestParameters _params;

        [Fact]
        public void QueryWithoutParametersShouldAssignCausationIdWhenCreated()
        {
            var subject = new Query<TestQueryResponse>(Constants.CorrelationId, Constants.CausationId);

            subject.CausationId.ToString().Should().Be(Constants.CausationId.ToString());
        }

        [Fact]
        public void QueryWithoutParametersShouldAssignCorrelationIdWhenCreated()
        {
            var subject = new Query<TestQueryResponse>(Constants.CorrelationId, Constants.CausationId);

            subject.CorrelationId.ToString().Should().Be(Constants.CorrelationId.ToString());
        }

        [Fact]
        public void QueryWithParametersShouldAssignCausationIdWhenCreated()
        {
            var subject = new Query<TestParameters, TestQueryResponse>(_params, Constants.CorrelationId, Constants.CausationId);

            subject.CausationId.ToString().Should().Be(Constants.CausationId.ToString());
        }

        [Fact]
        public void QueryWithParametersShouldAssignCorrelationIdWhenCreated()
        {
            var subject = new Query<TestParameters, TestQueryResponse>(_params, Constants.CorrelationId, Constants.CausationId);

            subject.CorrelationId.ToString().Should().Be(Constants.CorrelationId.ToString());
        }

        [Fact]
        public void QueryWithParametersShouldAssignMessageIdWhenCreated()
        {
            var subject = new Query<TestParameters, TestQueryResponse>(_params, Constants.CorrelationId, Constants.CausationId);

            subject.MessageId.Should().NotBeEmpty();
        }

        [Fact]
        public void QueryWithParametersShouldAssignParameters()
        {
            var subject = new Query<TestParameters, TestQueryResponse>(_params, Constants.CorrelationId, Constants.CausationId);

            subject.Params.Should().NotBeNull();
            subject.Params.Should().BeEquivalentTo(_params);
        }
    }
}
