using System.Collections.Generic;
using Blauhaus.Analytics.Abstractions.Config;
using Blauhaus.Analytics.Abstractions.Operation;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Analytics.Console.ConsoleLoggers;

namespace Blauhaus.Analytics.Console.Service
{
    public class ConsoleLoggerService: IAppInsightsClientService, IAppInsightsServerService, IAppInsightsService
    {
        protected readonly IConsoleLogger ConsoleLogger;

        public ConsoleLoggerService(
            IConsoleLogger consoleLogger)
        {
            ConsoleLogger = consoleLogger;
        }

        public IAnalyticsOperation CurrentOperation { get; private set; }
        public string CurrentSessionId { get; private set; }

        public IAnalyticsOperation StartOperation(string operationName)
        {
            CurrentOperation = new AnalyticsOperation(operationName, duration =>
            {
                ConsoleLogger.LogOperation(operationName, duration);
                CurrentOperation = null;
            });

            return CurrentOperation;
        }

        public IAnalyticsOperation ContinueOperation(string operationName)
        {
            if (CurrentOperation == null)
            {
                return StartOperation(operationName);
            }

            return new AnalyticsOperation(CurrentOperation, duration =>
            {
                ConsoleLogger.LogOperation(operationName, duration);
                CurrentOperation = null;
            });
        }

        public IAnalyticsOperation StartRequestOperation(string requestName, string operationName, string operationId, string sessionId)
        {
            CurrentSessionId = sessionId;

            CurrentOperation = new AnalyticsOperation(operationId, operationName, duration =>
            {
                ConsoleLogger.LogOperation(requestName, duration);
                CurrentOperation = null;
            });

            return CurrentOperation;
        }

        public IAnalyticsOperation StartPageViewOperation(string viewName)
        {
            CurrentOperation = new AnalyticsOperation(viewName, duration =>
            {
                ConsoleLogger.LogOperation(viewName, duration);
                CurrentOperation = null;
            });

            return CurrentOperation;
        }
        
        public void Trace(string message, LogSeverity logSeverityLevel = LogSeverity.Verbose, Dictionary<string, object> properties = null)
        {
            ConsoleLogger.LogTrace(message, logSeverityLevel, properties);
        }

        public void LogEvent(string eventName, Dictionary<string, object> properties = null, Dictionary<string, double> metrics = null)
        {
            ConsoleLogger.LogEvent(eventName, properties, metrics);
        }

    }
}