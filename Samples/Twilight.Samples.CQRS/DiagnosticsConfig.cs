using System.Diagnostics;

namespace Twilight.Samples.CQRS;

public static class DiagnosticsConfig
{
    public const string ServiceName = "Twilight.Samples.CQRS";

    public static readonly ActivitySource ActivitySource = new(ServiceName);
}