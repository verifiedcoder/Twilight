using System.Diagnostics;

namespace Twilight.Samples.CQRS;

public static class DiagnosticsConfig
{
    public const string ApplicationName = "Twilight.Samples.CQRS";

    public static readonly ActivitySource ActivitySource = new(ApplicationName);
}