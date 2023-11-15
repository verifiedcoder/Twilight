namespace Twilight.CQRS.Tests.Common;

public sealed class TestParameters(string value)
{
    public TestParameters()
        : this("test")
    {
    }

    public string Value { get; } = value;
}
