using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Blauhaus.Analytics.Abstractions.Extensions;
using Blauhaus.Analytics.Abstractions.Operation;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Analytics.Abstractions.Session;
using Blauhaus.Analytics.Console.ConsoleLoggers;
using Blauhaus.Common.ValueObjects.BuildConfigs;

namespace Blauhaus.Analytics.Console.Service
{
    public class ConsoleLoggerService: IAnalyticsService
    {
        private readonly IBuildConfig _currentBuildConfig;
        protected readonly IConsoleLogger ConsoleLogger;
        private IAnalyticsOperation? _currentOperation;
        private IAnalyticsSession? _currentSession;

        public ConsoleLoggerService(
            IBuildConfig currentBuildConfig,
            IConsoleLogger consoleLogger)
        {
            _currentBuildConfig = currentBuildConfig;
            ConsoleLogger = consoleLogger;
        }

        public IAnalyticsOperation? CurrentOperation
        {
            get => _currentOperation;
            protected set
            {
                _currentOperation?.Dispose();
                _currentOperation = value;
            }
        }

        public virtual IAnalyticsSession CurrentSession
        {
            get => _currentSession ??= AnalyticsSession.New;
            protected set => _currentSession = value;
        } 

        public IDictionary<string, string> AnalyticsOperationHeaders { get; } = new Dictionary<string, string>();

        public void ResetCurrentSession(string newSessionId = "")
        {
            CurrentOperation?.Dispose();
            CurrentOperation = null;
            var sessionId = CurrentSession.Id;
            CurrentSession = AnalyticsSession.FromExisting(sessionId);
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

        public IAnalyticsOperation StartPageViewOperation(object sender, string viewName = "", Dictionary<string, object>? properties = null, string callingMember = "")
        {
            if (string.IsNullOrWhiteSpace(viewName))
            {
                viewName = sender.GetType().Name;
            }  

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

        public IAnalyticsOperation StartTrace(object sender, string message, LogSeverity logSeverity = LogSeverity.Verbose, Dictionary<string, object> properties = null, string callerMemberName = "")
        {
            properties ??= new Dictionary<string, object>();
            return new AnalyticsOperation(message, duration =>
            {
                properties["Duration"] = duration;
                var p = properties.ToDictionaryOfStrings();
                ConsoleLogger.LogTrace(message, logSeverity, properties.ToDictionaryOfStrings());
            });
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

        public void LogEvent(object sender, string eventName, Dictionary<string, object>? properties = null, [CallerMemberName] string callerMemberName = "")
        {
            ConsoleLogger.LogEvent(eventName, properties.ToDictionaryOfStrings());
        }

        public void LogException(object sender, Exception exception, Dictionary<string, object>? properties = null, [CallerMemberName] string callerMemberName = "")
        {
            ConsoleLogger.LogException(exception, properties.ToDictionaryOfStrings());
        }

        public void Debug(string message, Dictionary<string, object>? properties = null)
        {
            if (_currentBuildConfig.Equals(BuildConfig.Debug))
            {
                ConsoleLogger.LogTrace(message, LogSeverity.Debug, properties.ToDictionaryOfStrings());
            }
        }
    }
}