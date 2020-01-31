using System.Collections.Generic;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Common.ValueObjects.BuildConfigs;

namespace Blauhaus.Analytics.Abstractions.Config
{
    public interface IApplicationInsightsConfig
    {
        string InstrumentationKey { get; }

        string RoleName { get; }

        Dictionary<IBuildConfig, LogSeverity> MinimumLogToServerSeverity { get; }



    }
}