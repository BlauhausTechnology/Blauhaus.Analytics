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
        protected readonly ITelemetryClientProxy TelemetryClient;


        protected BaseAppInsightsService(
            IApplicationInsightsConfig config, 
            IConsoleLogger appInsightsLogger, 
            ITelemetryClientProxy telemetryClient)
        {
            Config = config;
            ConsoleLogger = appInsightsLogger;
            TelemetryClient = telemetryClient;
        }

        protected ITelemetryClientProxy GetClient()
        {
            TelemetryClient.UpdateOperation(CurrentOperation, CurrentSessionId);
            return TelemetryClient;
        }

        public IAnalyticsOperation? CurrentOperation { get; protected set; }

        public string CurrentSessionId { get; protected set; } = string.Empty;

        public IAnalyticsOperation StartOperation(string operationName)
        {
            CurrentOperation = new AnalyticsOperation(operationName, duration =>
            {
                TelemetryClient.UpdateOperation(CurrentOperation, CurrentSessionId);

                var dependencyTelemetry = new DependencyTelemetry
                {
                    Duration = duration,
                    Name = operationName
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
                TelemetryClient.UpdateOperation(CurrentOperation, CurrentSessionId);

                var dependencyTelemetry = new DependencyTelemetry()
                {
                    Duration = duration,
                    Name = operationName
                };
                TelemetryClient.TrackDependency(dependencyTelemetry);
            });

        }
        
        //todo next: test request and page view dependencies then logging

        public void Trace(string message, SeverityLevel severityLevel = SeverityLevel.Verbose, Dictionary<string, object> properties = null)
        {
            //todo convert object into scalar or json 
            var client = GetClient();
            client.TrackTrace(message, severityLevel, new Dictionary<string, string>());
            ConsoleLogger.TrackTrace(message, severityLevel, properties);
        }

        public void LogEvent(string eventName, Dictionary<string, object> properties = null, Dictionary<string, double> metrics = null)
        {
            //todo convert object into scalar or json 
            var client = GetClient();
            client.TrackEvent(eventName, new Dictionary<string, string>(), metrics);
            ConsoleLogger.TrackEvent(eventName, properties, metrics);
        }
    }
}