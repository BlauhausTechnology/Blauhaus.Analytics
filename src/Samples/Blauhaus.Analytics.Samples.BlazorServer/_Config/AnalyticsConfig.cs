using System.Collections.Generic;
using Blauhaus.Analytics.Abstractions.Config;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Common.ValueObjects.BuildConfigs;
using Microsoft.Extensions.Configuration;

namespace Blauhaus.Analytics.Samples.BlazorServer._Config
{
    public class AnalyticsConfig : BaseApplicationInsightsConfig
    {
        public AnalyticsConfig(IConfiguration configuration) : base("4e647a09-071e-4cb8-85e4-303d5458d6fb", "Blazor Server")
        {
            MinimumLogToServerSeverity[BuildConfig.Debug] = LogSeverity.Critical;
            MinimumLogToServerSeverity[BuildConfig.Release] = LogSeverity.Information;
        }
    }
}