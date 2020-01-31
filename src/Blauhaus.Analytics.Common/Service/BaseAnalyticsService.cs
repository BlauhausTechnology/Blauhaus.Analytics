using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using Blauhaus.Analytics.Abstractions.Config;
using Blauhaus.Analytics.Abstractions.Operation;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Analytics.Abstractions.Session;
using Blauhaus.Analytics.Abstractions.TelemetryClients;
using Blauhaus.Analytics.Common.Extensions;
using Blauhaus.Analytics.Common.TelemetryClients;
using Blauhaus.Analytics.Console.ConsoleLoggers;
using Blauhaus.Common.ValueObjects.BuildConfigs;
using Microsoft.ApplicationInsights.DataContracts;
using Newtonsoft.Json;

namespace Blauhaus.Analytics.Common.Service
{
    public abstract class BaseAnalyticsService : IAnalyticsService
    {
        protected readonly IApplicationInsightsConfig Config;
        protected readonly IConsoleLogger ConsoleLogger;
        protected readonly IBuildConfig CurrentBuildConfig;

        private readonly ITelemetryClientProxy _telemetryClient;
        protected readonly ITelemetryDecorator TelemetryDecorator;

        protected ITelemetryClientProxy TelemetryClient => 
            _telemetryClient.UpdateOperation(CurrentOperation, CurrentSession);

        protected BaseAnalyticsService(
            IApplicationInsightsConfig config, 
            IConsoleLogger appInsightsLogger, 
            ITelemetryClientProxy telemetryClient, 
            ITelemetryDecorator telemetryDecorator,
            IBuildConfig currentBuildConfig)
        {
            Config = config;
            ConsoleLogger = appInsightsLogger;
            _telemetryClient = telemetryClient;
            TelemetryDecorator = telemetryDecorator;
            CurrentBuildConfig = currentBuildConfig;
        }


        public IAnalyticsOperation? CurrentOperation { get; protected set; }
        public IAnalyticsSession CurrentSession { get; protected set; } = AnalyticsSession.Empty;

        public IAnalyticsOperation StartOperation(string operationName, Dictionary<string, object>? properties = null)
        {

            CurrentOperation = new AnalyticsOperation(operationName, duration =>
            {
                var dependencyTelemetry = new DependencyTelemetry
                {
                    Duration = duration,
                    Name = operationName,
                };
                
                if(properties == null) properties = new Dictionary<string, object>();

                TelemetryDecorator.DecorateTelemetry(dependencyTelemetry, CurrentOperation, CurrentSession, properties.ToDictionaryOfStrings());
                TelemetryClient.TrackDependency(dependencyTelemetry);
                ConsoleLogger.LogOperation(operationName, duration);

                CurrentOperation = null;
            });

            return CurrentOperation;
        }

        public IAnalyticsOperation ContinueOperation(string operationName, Dictionary<string, object>? properties = null)
        {
            if (CurrentOperation == null)
            {
                return StartOperation(operationName, properties);
            }

            return new AnalyticsOperation(CurrentOperation, duration =>
            {
                var dependencyTelemetry = new DependencyTelemetry
                {
                    Duration = duration,
                    Name = operationName
                };

                if(properties == null) properties = new Dictionary<string, object>();
                
                TelemetryDecorator.DecorateTelemetry(dependencyTelemetry, CurrentOperation, CurrentSession, properties.ToDictionaryOfStrings());
                TelemetryClient.TrackDependency(dependencyTelemetry);
                ConsoleLogger.LogOperation(operationName, duration);
            });

        }
        
        public void Trace(string message, LogSeverity logSeverity = LogSeverity.Verbose, Dictionary<string, object> properties = null)
        {
            if (Config.MinimumLogToServerSeverity.TryGetValue(CurrentBuildConfig, out var minumumSeverityToLogToServer))
            {
                if (logSeverity >= minumumSeverityToLogToServer)
                {
                    TelemetryClient.TrackTrace(message, (SeverityLevel) logSeverity, properties.ToDictionaryOfStrings());
                }
            }

            ConsoleLogger.LogTrace(message, logSeverity, properties);
        }   

        public void LogEvent(string eventName, Dictionary<string, object> properties = null, Dictionary<string, double> metrics = null)
        {
            var telemetry = new EventTelemetry(eventName);
            TelemetryDecorator.DecorateTelemetry(telemetry, CurrentOperation, CurrentSession, properties.ToDictionaryOfStrings());
            TelemetryClient.TrackEvent(telemetry);
            
            ConsoleLogger.LogEvent(eventName, properties, metrics);
        }

        public void LogException(Exception exception, Dictionary<string, object> properties = null, Dictionary<string, double> metrics = null)
        {
            TelemetryClient.TrackException(exception, properties.ToDictionaryOfStrings(), metrics);
            ConsoleLogger.LogException(exception, properties, metrics);
        }
    }
}