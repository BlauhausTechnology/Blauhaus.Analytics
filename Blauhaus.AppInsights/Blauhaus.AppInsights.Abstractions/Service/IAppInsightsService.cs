using System.Collections.Generic;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Blauhaus.AppInsights.Abstractions.Operation;

namespace Blauhaus.AppInsights.Abstractions.Service
{
    public interface IAppInsightsService
    {
        IAnalyticsOperation? CurrentOperation { get; }
        string CurrentSessionId { get; }

        IAnalyticsOperation StartOperation(string operationName);
        IAnalyticsOperation StartOrContinueOperation(string operationName);

        void Trace(string message, SeverityLevel severityLevel = 0, Dictionary<string, string> properties = null);
        void LogEvent(string eventName, Dictionary<string, string> properties = null, Dictionary<string, double> metrics = null);

    }
}