namespace Twilight.CQRS.Tests.Common;

public sealed class TestParameters
{
    public TestParameters()
        => Value = "test";

    public TestParameters(string value)
        => Value = value;

    public string Value { get; }
}
