using System;
using System.Collections.Generic;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Blauhaus.AppInsights.Abstractions.Operation;

namespace Blauhaus.AppInsights.Abstractions.Service
{
    public abstract class BaseAppInsightsService : IAppInsightsService
    {
        protected abstract TelemetryClient GetClient();

        public IAnalyticsOperation? CurrentOperation { get; protected set; }

        public string CurrentSessionId { get; protected set; } = string.Empty;

        public IAnalyticsOperation StartOperation(string operationName)
        {
            var operationId = Guid.NewGuid().ToString();

            CurrentOperation = new AnalyticsOperation(operationId, operationName, duration =>
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
                var operationId = Guid.NewGuid().ToString();

                CurrentOperation = new AnalyticsOperation(operationId, operationName, duration =>
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
            
        }

        public void LogEvent(string eventName,  Dictionary<string, string> properties = null, Dictionary<string, double> metrics = null)
        {
            var client = GetClient();
            client.TrackEvent(eventName, properties, metrics);
        }
    }
}