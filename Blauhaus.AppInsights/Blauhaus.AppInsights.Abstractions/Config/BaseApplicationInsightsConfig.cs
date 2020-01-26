using System.Collections.Generic;
using Blauhaus.Common.ValueObjects.BuildConfigs;
using Microsoft.ApplicationInsights.DataContracts;

namespace Blauhaus.AppInsights.Abstractions.Config
{
    public abstract class BaseApplicationInsightsConfig : IApplicationInsightsConfig
    {
        protected BaseApplicationInsightsConfig(string instrumentationKey)
        {
            InstrumentationKey = instrumentationKey;

            MinimumLogToConsoleSeverity = new Dictionary<IBuildConfig, SeverityLevel>
            {
                {BuildConfig.Debug, SeverityLevel.Verbose },
                {BuildConfig.Release, SeverityLevel.Critical }
            };

            MinimumLogToServerSeverity = new Dictionary<IBuildConfig, SeverityLevel>
            {
                {BuildConfig.Debug, SeverityLevel.Critical },
                {BuildConfig.Release, SeverityLevel.Information }
            };

        }

        public string InstrumentationKey { get; }

        public Dictionary<IBuildConfig, SeverityLevel> MinimumLogToConsoleSeverity { get; }

        public Dictionary<IBuildConfig, SeverityLevel> MinimumLogToServerSeverity { get; }
    }
}