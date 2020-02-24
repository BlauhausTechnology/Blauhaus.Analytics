using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Blauhaus.Analytics.Abstractions.Extensions;
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
        public IDictionary<string, string> AnalyticsOperationHeaders { get; } = new Dictionary<string, string>();

        public void ClearCurrentSession()
        {
            CurrentSession = AnalyticsSession.New;
        }

        public IAnalyticsOperation StartRequestOperation(object sender, string requestName, IDictionary<string, string> headers, string callingMember = "")
        {
            CurrentOperation = new AnalyticsOperation(requestName, duration =>
            {
                ConsoleLogger.LogOperation(requestName, duration);
                CurrentOperation = null;
            });

            return CurrentOperation;
        }

        public IAnalyticsOperation StartPageViewOperation(object sender, string viewName, string callingMember = "")
        {
              
            CurrentOperation = new AnalyticsOperation(viewName, duration =>
            {
                ConsoleLogger.LogOperation(viewName, duration);
                CurrentOperation = null;
            });

            return CurrentOperation;
        }

        public IAnalyticsOperation StartOperation(object sender, string operationName, Dictionary<string, object> properties = null, [CallerMemberName] string callerMemberName = "")
        {
            
            CurrentOperation = new AnalyticsOperation(operationName, duration =>
            {
                ConsoleLogger.LogOperation(operationName, duration);
                CurrentOperation = null;
            });

            return CurrentOperation;
        }

        public IAnalyticsOperation ContinueOperation(object sender, string operationName, Dictionary<string, object> properties = null, [CallerMemberName] string callerMemberName = "")
        {
            if (CurrentOperation == null)
            {
                return StartOperation(sender, operationName, properties, callerMemberName);
            }

            return new AnalyticsOperation(CurrentOperation, duration =>
            {
                ConsoleLogger.LogOperation(operationName, duration);
                CurrentOperation = null;
            });
        }

        public void Trace(object sender, string message, LogSeverity logSeverityLevel = LogSeverity.Verbose, Dictionary<string, object>? properties = null, [CallerMemberName] string callerMemberName = "")
        {
            ConsoleLogger.LogTrace(message, logSeverityLevel, properties.ToDictionaryOfStrings());
        }

        public void LogEvent(object sender, string eventName, Dictionary<string, object>? properties = null, Dictionary<string, double>? metrics = null, [CallerMemberName] string callerMemberName = "")
        {
            ConsoleLogger.LogEvent(eventName, properties.ToDictionaryOfStrings(), metrics);
        }

        public void LogException(object sender, Exception exception, Dictionary<string, object>? properties = null, Dictionary<string, double>? metrics = null, [CallerMemberName] string callerMemberName = "")
        {
            ConsoleLogger.LogException(exception, properties.ToDictionaryOfStrings(), metrics);
        }
    }
}