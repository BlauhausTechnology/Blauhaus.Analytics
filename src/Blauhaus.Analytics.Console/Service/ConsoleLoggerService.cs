using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Blauhaus.Analytics.Abstractions.Http;
using Blauhaus.Analytics.Abstractions.Operation;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Analytics.Console.ConsoleLoggers;

namespace Blauhaus.Analytics.Console.Service
{
    public class ConsoleLoggerService: IAnalyticsClientService, IAnalyticsServerService, IAnalyticsService
    {
        protected readonly IConsoleLogger ConsoleLogger;

        public ConsoleLoggerService(
            IConsoleLogger consoleLogger)
        {
            ConsoleLogger = consoleLogger;
        }

        public IAnalyticsOperation? CurrentOperation { get; private set; }
        public string? CurrentSessionId { get; private set; }

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

        public IAnalyticsOperation StartRequestOperation(string requestName, IDictionary<string, string> headers)
        {
            if (!headers.TryGetValue(AnalyticsHeaders.OperationName, out var operationNames))
            {
                throw new ArgumentException(AnalyticsHeaders.OperationName + " missing from request headers");
            }
            
            if (!headers.TryGetValue(AnalyticsHeaders.OperationId, out var operationIds))
            {
                throw new ArgumentException(AnalyticsHeaders.OperationId + " missing from request headers");
            }
            
            if (!headers.TryGetValue(AnalyticsHeaders.SessionId, out var sessionId))
            {
                throw new ArgumentException(AnalyticsHeaders.SessionId + " missing from request headers");
            }

            return StartRequestOperation(requestName, operationNames, operationIds, sessionId);
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

        public HttpRequestHeaders AddAnalyticsHeaders(HttpRequestHeaders headers)
        {
            return headers;
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