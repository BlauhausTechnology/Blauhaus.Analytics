using Blauhaus.Common.ValueObjects.BuildConfigs;
using Microsoft.ApplicationInsights.DataContracts;
using System.Collections.Generic;

namespace Blauhaus.AppInsights.Abstractions.Config
{
    public abstract class BaseApplicationInsightsConfig : IApplicationInsightsConfig
    {
        protected BaseApplicationInsightsConfig(string instrumentationKey, string clientName)
        {
            InstrumentationKey = instrumentationKey;
            ClientName = clientName;

            MinimumLogToServerSeverity = new Dictionary<IBuildConfig, SeverityLevel>
            {
                {BuildConfig.Debug, SeverityLevel.Critical },
                {BuildConfig.Release, SeverityLevel.Information }
            };


        }

        public string InstrumentationKey { get; }
        public string ClientName { get; }


        public Dictionary<IBuildConfig, SeverityLevel> MinimumLogToServerSeverity { get; }
    }
}