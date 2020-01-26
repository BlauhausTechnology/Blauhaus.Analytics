using System.Collections.Generic;
using Microsoft.ApplicationInsights.DataContracts;

namespace Blauhaus.AppInsights.Abstractions.TelemetryClients
{
    public interface IAppInsightsLogger
    {
        
        void TrackTrace(string message, SeverityLevel severityLevel, Dictionary<string, string> properties);
        void TrackEvent(string eventName, Dictionary<string, string> properties, Dictionary<string, double> metrics);
    }
}