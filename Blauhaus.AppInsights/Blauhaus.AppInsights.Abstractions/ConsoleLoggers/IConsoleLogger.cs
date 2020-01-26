using Microsoft.ApplicationInsights.DataContracts;
using System.Collections.Generic;
using Blauhaus.AppInsights.Abstractions.TelemetryClients;

namespace Blauhaus.AppInsights.Abstractions.ConsoleLoggers
{
    public interface IConsoleLogger
    {
        
        void TrackTrace(string message, SeverityLevel severityLevel, Dictionary<string, object> properties);
        void TrackEvent(string eventName, Dictionary<string, object> properties, Dictionary<string, double> metrics);

    }
}
