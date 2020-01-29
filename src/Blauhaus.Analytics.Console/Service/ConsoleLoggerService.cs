using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Blauhaus.Analytics.Abstractions.Http;
using Blauhaus.Analytics.Abstractions.Operation;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Analytics.Abstractions.Session;
using Blauhaus.Analytics.Console.ConsoleLoggers;

namespace Blauhaus.Analytics.Console.Service
{
    public class ConsoleLoggerService: IAnalyticsService
    {
        protected readonly IConsoleLogger ConsoleLogger;

        public ConsoleLoggerService(
            IConsoleLogger consoleLogger)
        {
            ConsoleLogger = consoleLogger;
        }

        public IAnalyticsOperation? CurrentOperation { get; private set; }
        public IAnalyticsSession CurrentSession { get; private set; }

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

        public void Trace(string message, LogSeverity logSeverityLevel = LogSeverity.Verbose, Dictionary<string, object>? properties = null)
        {
            ConsoleLogger.LogTrace(message, logSeverityLevel, properties);
        }

        public void LogEvent(string eventName, Dictionary<string, object>? properties = null, Dictionary<string, double>? metrics = null)
        {
            ConsoleLogger.LogEvent(eventName, properties, metrics);
        }

        public void LogException(Exception exception, Dictionary<string, object>? properties = null, Dictionary<string, double>? metrics = null)
        {
            ConsoleLogger.LogException(exception, properties, metrics);
        }
    }
}