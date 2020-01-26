using Blauhaus.Common.ValueObjects.BuildConfigs;
using Microsoft.ApplicationInsights.DataContracts;
using System.Collections.Generic;

namespace Blauhaus.AppInsights.Abstractions.Config
{
    public interface IApplicationInsightsConfig
    {
        string InstrumentationKey { get; }

        string ClientName { get; }

        Dictionary<IBuildConfig, SeverityLevel> MinimumLogToServerSeverity { get; }



    }
}