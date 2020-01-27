using Blauhaus.AppInsights.Abstractions.Config;
using Blauhaus.AppInsights.Abstractions.ConsoleLoggers;
using Blauhaus.AppInsights.Abstractions.Operation;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using System.Collections.Generic;
using Blauhaus.AppInsights.Abstractions.TelemetryClients;
using Blauhaus.Common.ValueObjects.BuildConfigs;
using Microsoft.ApplicationInsights.Channel;

namespace Blauhaus.AppInsights.Abstractions.Service
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

        public void Trace(string message, Severity severity = Severity.Verbose, Dictionary<string, object> properties = null)
        {
            var severityLevel = (SeverityLevel) severity;
            //todo convert object into scalar or json 
            TelemetryClient.TrackTrace(message, severityLevel, new Dictionary<string, string>());
            ConsoleLogger.TrackTrace(message, severityLevel, properties);
        }

        public void LogEvent(string eventName, Dictionary<string, object> properties = null, Dictionary<string, double> metrics = null)
        {
            //todo convert object into scalar or json 
            TelemetryClient.TrackEvent(eventName, new Dictionary<string, string>(), metrics);
            ConsoleLogger.TrackEvent(eventName, properties, metrics);
        }
    }
}