using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Blauhaus.Analytics.Abstractions.Config;
using Blauhaus.Analytics.Abstractions.Extensions;
using Blauhaus.Analytics.Abstractions.Operation;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Analytics.Abstractions.Session;
using Blauhaus.Analytics.Common.Telemetry;
using Blauhaus.Analytics.Console.ConsoleLoggers;
using Blauhaus.Common.ValueObjects.BuildConfigs;
using Microsoft.ApplicationInsights.DataContracts;

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

        
        public IAnalyticsOperation StartPageViewOperation(object sender, string pageName, [CallerMemberName] string callerMember = "")
        {
            CurrentOperation = new AnalyticsOperation(pageName, duration =>
            {
                var pageViewTelemetry = new PageViewTelemetry(pageName)
                {
                    Duration = duration
                };

                TelemetryClient.TrackPageView(TelemetryDecorator.DecorateTelemetry(pageViewTelemetry, sender.GetType().Name, callerMember, CurrentOperation, CurrentSession,
                    new Dictionary<string, object>(), new Dictionary<string, double>()));

                ConsoleLogger.LogOperation(pageName, duration);
                
                CurrentOperation = null;
            });

            LogTrace($"{pageName} started", LogSeverity.Verbose, new Dictionary<string, object>(), sender.GetType().Name, callerMember);

            return CurrentOperation;
        }


        public IAnalyticsOperation StartOperation(object sender, string operationName, Dictionary<string, object>? properties = null, [CallerMemberName] string callerMemberName = "")
        {
            var callingClassName = sender.GetType().Name;

            CurrentOperation = new AnalyticsOperation(operationName, duration =>
            {
                var dependencyTelemetry = new DependencyTelemetry
                {
                    Duration = duration,
                    Name = operationName,
                };
                
                if(properties == null) properties = new Dictionary<string, object>();

                TelemetryDecorator.DecorateTelemetry(dependencyTelemetry, callingClassName, callerMemberName, CurrentOperation, CurrentSession, properties);
                TelemetryClient.TrackDependency(dependencyTelemetry);
                ConsoleLogger.LogOperation(operationName, duration);

                CurrentOperation = null;
            });

            return CurrentOperation;
        }

        public IAnalyticsOperation ContinueOperation(object sender, string operationName, Dictionary<string, object>? properties = null, [CallerMemberName] string callerMemberName = "")
        {
            var callingClassName = sender.GetType().Name;

            if (CurrentOperation == null)
            {
                return StartOperation(sender, operationName, properties, callerMemberName);
            }

            return new AnalyticsOperation(CurrentOperation, duration =>
            {
                var dependencyTelemetry = new DependencyTelemetry
                {
                    Duration = duration,
                    Name = operationName
                };

                if(properties == null) properties = new Dictionary<string, object>();
                
                TelemetryDecorator.DecorateTelemetry(dependencyTelemetry, callingClassName, callerMemberName, CurrentOperation, CurrentSession, properties);
                TelemetryClient.TrackDependency(dependencyTelemetry);
                ConsoleLogger.LogOperation(operationName, duration);
            });

        }
        
        public void LogEvent(object sender, string eventName, Dictionary<string, object> properties = null, Dictionary<string, double> metrics = null, [CallerMemberName] string callerMemberName = "")
        {
            TelemetryClient.TrackEvent(TelemetryDecorator
                .DecorateTelemetry(new EventTelemetry(eventName), sender.GetType().Name, callerMemberName, CurrentOperation, CurrentSession, properties, metrics));

            ConsoleLogger.LogEvent(eventName, properties.ToDictionaryOfStrings(), metrics);
        }

        public void LogException(object sender, Exception exception, Dictionary<string, object> properties = null, Dictionary<string, double> metrics = null, [CallerMemberName] string callerMemberName = "")
        {
            TelemetryClient.TrackException(TelemetryDecorator
                .DecorateTelemetry(new ExceptionTelemetry(exception), sender.GetType().Name, callerMemberName, CurrentOperation, CurrentSession, properties, metrics));
            
            ConsoleLogger.LogException(exception, properties.ToDictionaryOfStrings(), metrics);
        }


        public void Trace(object sender, string message, LogSeverity logSeverity = LogSeverity.Verbose, Dictionary<string, object> properties = null, [CallerMemberName] string callerMemberName = "")
        {
            LogTrace(message, logSeverity, properties, sender.GetType().Name, callerMemberName);
        }   

        protected void LogTrace(string message, LogSeverity logSeverity, Dictionary<string, object> properties, string callingClassName, string callerMemberName)
        {
            if (Config.MinimumLogToServerSeverity.TryGetValue(CurrentBuildConfig, out var minumumSeverityToLogToServer))
            {
                if (logSeverity >= minumumSeverityToLogToServer)
                {
                    TelemetryClient.TrackTrace(TelemetryDecorator
                        .DecorateTelemetry(new TraceTelemetry(message), callingClassName, callerMemberName, CurrentOperation, CurrentSession, properties));
                }
            }

            ConsoleLogger.LogTrace(message, logSeverity, properties.ToDictionaryOfStrings());
        }   



    }

    
}