using System.Collections.Generic;
using Blauhaus.Analytics.Abstractions.Config;
using Blauhaus.Analytics.Abstractions.Operation;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Analytics.Abstractions.TelemetryClients;
using Blauhaus.Analytics.Console.ConsoleLoggers;
using Microsoft.ApplicationInsights.DataContracts;

namespace Blauhaus.Analytics.Common.Service
{
    public abstract class BaseAppInsightsService : IAppInsightsService
    {
        protected readonly IApplicationInsightsConfig Config;
        protected readonly IConsoleLogger ConsoleLogger;

        private readonly ITelemetryClientProxy _telemetryClient;
        protected ITelemetryClientProxy TelemetryClient => _telemetryClient.UpdateOperation(CurrentOperation, CurrentSessionId);


        protected BaseAppInsightsService(
            IApplicationInsightsConfig config, 
            IConsoleLogger appInsightsLogger, 
            ITelemetryClientProxy telemetryClient)
        {
            Config = config;
            ConsoleLogger = appInsightsLogger;
            _telemetryClient = telemetryClient;
        }


        public IAnalyticsOperation? CurrentOperation { get; protected set; }

        public string CurrentSessionId { get; protected set; } = string.Empty;

        public IAnalyticsOperation StartOperation(string operationName)
        {
            CurrentOperation = new AnalyticsOperation(operationName, duration =>
            {
                var dependencyTelemetry = new DependencyTelemetry
                {
                    Duration = duration,
                    Name = operationName,
                    
                };

                TelemetryClient.TrackDependency(dependencyTelemetry);
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

            return new AnalyticsOperation(CurrentOperation.Id, CurrentOperation.Name, duration =>
            {
                var dependencyTelemetry = new DependencyTelemetry()
                {
                    Duration = duration,
                    Name = operationName
                };
                TelemetryClient.TrackDependency(dependencyTelemetry);
            });

        }
        
        //todo next: test request and page view dependencies then logging

        public void Trace(string message, LogSeverity logSeverity = LogSeverity.Verbose, Dictionary<string, object> properties = null)
        {
            //todo convert object into scalar or json 
            TelemetryClient.TrackTrace(message, (SeverityLevel) logSeverity, new Dictionary<string, string>());
            ConsoleLogger.TrackTrace(message, logSeverity, properties);
        }

        public void LogEvent(string eventName, Dictionary<string, object> properties = null, Dictionary<string, double> metrics = null)
        {
            //todo convert object into scalar or json 
            TelemetryClient.TrackEvent(eventName, new Dictionary<string, string>(), metrics);
            ConsoleLogger.TrackEvent(eventName, properties, metrics);
        }
    }
}