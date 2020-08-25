namespace Twilight.CQRS.Tests.Unit.Shared
{
    public sealed class NonValidatingTestParameters
    {
        public NonValidatingTestParameters(string value) => Value = value;

        public string Value { get; }
    }
}
