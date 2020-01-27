using Blauhaus.AppInsights.Abstractions.Operation;
using System.Collections.Generic;

namespace Blauhaus.AppInsights.Abstractions.Service
{
    public interface IAppInsightsService
    {
        IAnalyticsOperation? CurrentOperation { get; }
        string CurrentSessionId { get; }

        IAnalyticsOperation StartOperation(string operationName);
        IAnalyticsOperation ContinueOperation(string operationName);

        void Trace(string message, Severity severityLevel = 0, Dictionary<string, object>? properties = null);
        void LogEvent(string eventName, Dictionary<string, object>? properties = null, Dictionary<string, double>? metrics = null);

    }
}