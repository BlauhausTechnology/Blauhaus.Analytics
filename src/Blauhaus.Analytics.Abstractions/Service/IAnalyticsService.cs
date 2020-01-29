using System;
using System.Collections.Generic;
using Blauhaus.Analytics.Abstractions.Operation;
using Blauhaus.Analytics.Abstractions.Session;

namespace Blauhaus.Analytics.Abstractions.Service
{
    public interface IAnalyticsService
    {
        IAnalyticsOperation? CurrentOperation { get; }
        AnalyticsSession CurrentSession { get; }

        IAnalyticsOperation StartOperation(string operationName);
        IAnalyticsOperation ContinueOperation(string operationName);

        void Trace(string message, LogSeverity logSeverityLevel = 0, Dictionary<string, object>? properties = null);
        void LogEvent(string eventName, Dictionary<string, object>? properties = null, Dictionary<string, double>? metrics = null);
        void LogException(Exception exception, Dictionary<string, object>? properties = null, Dictionary<string, double>? metrics = null);

    }
}