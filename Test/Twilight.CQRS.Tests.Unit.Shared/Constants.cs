using System;

namespace Twilight.CQRS.Tests.Unit.Shared
{
    public static class Constants
    {
        public static string CorrelationId => Guid.Parse("02F9310E-13B1-4DFD-9ACF-7D55DA65D071").ToString();

        public static string CausationId => Guid.Parse("02F9310E-13B1-4DFD-9ACF-7D55DA65D072").ToString();
    }
}
