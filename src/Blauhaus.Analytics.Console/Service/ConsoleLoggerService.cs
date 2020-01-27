using System.Collections.Generic;
using Blauhaus.Analytics.Abstractions.Operation;
using Blauhaus.Analytics.Abstractions.Service;

namespace Blauhaus.Analytics.Console.Service
{
    public class ConsoleLoggerService: IAppInsightsClientService, IAppInsightsServerService
    {
        public IAnalyticsOperation CurrentOperation { get; }
        public string CurrentSessionId { get; }

        public IAnalyticsOperation StartOperation(string operationName)
        {
            throw new System.NotImplementedException();
        }

        public IAnalyticsOperation ContinueOperation(string operationName)
        {
            throw new System.NotImplementedException();
        }

        public void Trace(string message, LogSeverity logSeverityLevel = LogSeverity.Verbose, Dictionary<string, object> properties = null)
        {
            throw new System.NotImplementedException();
        }

        public void LogEvent(string eventName, Dictionary<string, object> properties = null, Dictionary<string, double> metrics = null)
        {
            throw new System.NotImplementedException();
        }

        public IAnalyticsOperation StartRequestOperation(string requestName, string operationId, string operationName, string sessionId)
        {
            throw new System.NotImplementedException();
        }

        public IAnalyticsOperation StartPageViewOperation(string viewName)
        {
            throw new System.NotImplementedException();
        }
    }
}