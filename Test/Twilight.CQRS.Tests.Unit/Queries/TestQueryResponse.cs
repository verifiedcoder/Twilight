namespace Twilight.CQRS.Tests.Unit.Queries
{
    public sealed class TestQueryResponse
    {
        public TestQueryResponse() => Value = string.Empty;

        public string Value { get; init; }
    }
}
