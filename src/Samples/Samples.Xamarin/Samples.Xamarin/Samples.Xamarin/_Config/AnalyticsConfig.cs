using Blauhaus.Analytics.Abstractions.Config;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Common.ValueObjects.BuildConfigs;

namespace Samples.Xamarin._Config
{
    public class AnalyticsConfig : BaseApplicationInsightsConfig
    {
        public AnalyticsConfig() : base("4e647a09-071e-4cb8-85e4-303d5458d6fb", "Xamarin App")
        {
            MinimumLogToServerSeverity[BuildConfig.Debug] = LogSeverity.Verbose;
            MinimumLogToServerSeverity[BuildConfig.Release] = LogSeverity.Verbose;
        }
    }
}