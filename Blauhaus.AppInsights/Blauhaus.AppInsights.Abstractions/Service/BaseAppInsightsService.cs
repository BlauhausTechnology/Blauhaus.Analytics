using Blauhaus.AppInsights.Abstractions.Config;
using Blauhaus.AppInsights.Abstractions.ConsoleLoggers;
using Blauhaus.AppInsights.Abstractions.Operation;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using System.Collections.Generic;
using Blauhaus.AppInsights.Abstractions.TelemetryClients;
using Microsoft.ApplicationInsights.Channel;

namespace Blauhaus.AppInsights.Abstractions.Service
{
    public abstract class BaseAppInsightsService : IAppInsightsService
    {
        protected readonly IApplicationInsightsConfig Config;
        protected readonly IConsoleLogger AppInsightsLogger;
        protected readonly ITelemetryClientProxy TelemetryClient;

        protected abstract TelemetryClient ConstructTelementryClient();


        protected BaseAppInsightsService(IApplicationInsightsConfig config, IConsoleLogger appInsightsLogger, ITelemetryClientProxy telemetryClient)
        {
            Config = config;
            AppInsightsLogger = appInsightsLogger;
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
                var client = GetClient();

                var dependencyTelemetry = new DependencyTelemetry()
                {
                    Duration = duration,
                    Name = operationName
                };
                client.TrackDependency(dependencyTelemetry);
                client.Flush();

                CurrentOperation = null;
            });

            return CurrentOperation;
        }

        public IAnalyticsOperation StartOrContinueOperation(string operationName)
        {
            if (CurrentOperation == null)
            {
                CurrentOperation = new AnalyticsOperation(operationName, duration =>
                {
                    var client = GetClient();

                    var dependencyTelemetry = new DependencyTelemetry()
                    {
                        Duration = duration,
                        Name = operationName
                    };
                    client.TrackDependency(dependencyTelemetry);
                    client.Flush();
                    CurrentOperation = null;
                });
            }

            else
            {
                var tempOperation = new AnalyticsOperation(CurrentOperation.Id, CurrentOperation.Name, duration =>
                {
                    var client = GetClient();

                    var dependencyTelemetry = new DependencyTelemetry()
                    {
                        Duration = duration,
                        Name = operationName
                    };
                    client.TrackDependency(dependencyTelemetry);
                    client.Flush();
                });

                return tempOperation;
            }

            return CurrentOperation;
        }

        public void Trace(string message, SeverityLevel severityLevel = SeverityLevel.Verbose, Dictionary<string, string> properties = null)
        {
            var client = GetClient();
            client.TrackTrace(message, severityLevel, properties);
            AppInsightsLogger.TrackTrace(message, severityLevel, properties);
        }

        public void LogEvent(string eventName, Dictionary<string, string> properties = null, Dictionary<string, double> metrics = null)
        {
            var client = GetClient();
            client.TrackEvent(eventName, properties, metrics);
            AppInsightsLogger.TrackEvent(eventName, properties, metrics);
        }
    }
}