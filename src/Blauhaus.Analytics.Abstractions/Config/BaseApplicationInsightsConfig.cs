using System.Collections.Generic;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Common.ValueObjects.BuildConfigs;

namespace Blauhaus.Analytics.Abstractions.Config
{
    public abstract class BaseApplicationInsightsConfig : IApplicationInsightsConfig
    {
        protected BaseApplicationInsightsConfig(string instrumentationKey, string clientName)
        {
            InstrumentationKey = instrumentationKey;
            ClientName = clientName;

            MinimumLogToServerSeverity = new Dictionary<IBuildConfig, LogSeverity>
            {
                {BuildConfig.Debug, LogSeverity.Critical },
                {BuildConfig.Release, LogSeverity.Information }
            };


        }

        public string InstrumentationKey { get; }
        public string ClientName { get; }


        public Dictionary<IBuildConfig, LogSeverity> MinimumLogToServerSeverity { get; }
    }
}