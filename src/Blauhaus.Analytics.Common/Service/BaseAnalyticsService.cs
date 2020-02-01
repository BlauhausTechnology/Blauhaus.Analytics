using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using Blauhaus.Analytics.Abstractions.Config;
using Blauhaus.Analytics.Abstractions.Extensions;
using Blauhaus.Analytics.Abstractions.Operation;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Analytics.Abstractions.Session;
using Blauhaus.Analytics.Common.Telemetry;
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
        protected readonly ITelemetryClientProxy TelemetryClient;
        protected readonly ITelemetryDecorator TelemetryDecorator;

        protected BaseAnalyticsService(
            IApplicationInsightsConfig config, 
            IConsoleLogger consoleLogger, 
            ITelemetryClientProxy telemetryClient, 
            ITelemetryDecorator telemetryDecorator,
            IBuildConfig currentBuildConfig)
        {
            Config = config;
            ConsoleLogger = consoleLogger;
            TelemetryClient = telemetryClient;
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

                TelemetryDecorator.DecorateTelemetry(dependencyTelemetry, CurrentOperation, CurrentSession, properties);
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
                
                TelemetryDecorator.DecorateTelemetry(dependencyTelemetry, CurrentOperation, CurrentSession, properties);
                TelemetryClient.TrackDependency(dependencyTelemetry);
                ConsoleLogger.LogOperation(operationName, duration);
            });

        }
        
        public void LogEvent(string eventName, Dictionary<string, object> properties = null, Dictionary<string, double> metrics = null)
        {
            TelemetryClient.TrackEvent(TelemetryDecorator
                .DecorateTelemetry(new EventTelemetry(eventName), CurrentOperation, CurrentSession, properties, metrics));

            ConsoleLogger.LogEvent(eventName, properties.ToDictionaryOfStrings(), metrics);
        }

        public void LogException(Exception exception, Dictionary<string, object> properties = null, Dictionary<string, double> metrics = null)
        {
            TelemetryClient.TrackException(TelemetryDecorator
                .DecorateTelemetry(new ExceptionTelemetry(exception), CurrentOperation, CurrentSession, properties, metrics));
            
            ConsoleLogger.LogException(exception, properties.ToDictionaryOfStrings(), metrics);
        }


        public void Trace(string message, LogSeverity logSeverity = LogSeverity.Verbose, Dictionary<string, object> properties = null)
        {
            if (Config.MinimumLogToServerSeverity.TryGetValue(CurrentBuildConfig, out var minumumSeverityToLogToServer))
            {
                if (logSeverity >= minumumSeverityToLogToServer)
                {
                    TelemetryClient.TrackTrace(TelemetryDecorator
                        .DecorateTelemetry(new TraceTelemetry(message), CurrentOperation, CurrentSession, properties));
                }
            }

            ConsoleLogger.LogTrace(message, logSeverity, properties.ToDictionaryOfStrings());
        }   

    }
}