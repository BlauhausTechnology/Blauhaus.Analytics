using Blauhaus.AppInsights.Abstractions.Config;

namespace Blauhaus.AppInsights.Test.Tests.Config
{
    public class TestAppInsightsConfig : IApplicationInsightsConfig
    {
        public string InstrumentationKey { get; } = "4e647a09-071e-4cb8-85e4-303d5458d6fb";
    }
}