using System.Collections.Generic;
using Blauhaus.Common.ValueObjects.BuildConfigs;
using Microsoft.ApplicationInsights.DataContracts;

namespace Blauhaus.AppInsights.Abstractions.Config
{
    public interface IApplicationInsightsConfig
    {
        string InstrumentationKey { get; }

        Dictionary<IBuildConfig, SeverityLevel> MinimumLogToServerSeverity { get; }



    }
}