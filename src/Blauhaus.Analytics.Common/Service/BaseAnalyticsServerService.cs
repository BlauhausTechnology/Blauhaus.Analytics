using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using Blauhaus.Analytics.Abstractions.Config;
using Blauhaus.Analytics.Abstractions.Operation;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Analytics.Abstractions.TelemetryClients;
using Blauhaus.Analytics.Console.ConsoleLoggers;
using Blauhaus.Common.ValueObjects.BuildConfigs;
using Microsoft.ApplicationInsights.DataContracts;
using Newtonsoft.Json;

namespace Blauhaus.Analytics.Common.Service
{
    public abstract class BaseAnalyticsServerService : IAnalyticsService
    {
        protected readonly IApplicationInsightsConfig Config;
        protected readonly IConsoleLogger ConsoleLogger;
        protected readonly IBuildConfig CurrentBuildConfig;

        private readonly ITelemetryClientProxy _telemetryClient;
        protected ITelemetryClientProxy TelemetryClient => _telemetryClient.UpdateOperation(CurrentOperation, CurrentSessionId);

        protected BaseAnalyticsServerService(
            IApplicationInsightsConfig config, 
            IConsoleLogger appInsightsLogger, 
            ITelemetryClientProxy telemetryClient, 
            IBuildConfig currentBuildConfig)
        {
            Config = config;
            ConsoleLogger = appInsightsLogger;
            _telemetryClient = telemetryClient;
            CurrentBuildConfig = currentBuildConfig;
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

            return new AnalyticsOperation(CurrentOperation, duration =>
            {
                var dependencyTelemetry = new DependencyTelemetry()
                {
                    Duration = duration,
                    Name = operationName
                };
                TelemetryClient.TrackDependency(dependencyTelemetry);
            });

        }
        
        public void Trace(string message, LogSeverity logSeverity = LogSeverity.Verbose, Dictionary<string, object> properties = null)
        {

            if (properties == null)
            {
                properties = new Dictionary<string, object>();
            }

            if (Config.MinimumLogToServerSeverity.TryGetValue(CurrentBuildConfig, out var minumumSeverityToLogToServer))
            {
                if (logSeverity >= minumumSeverityToLogToServer)
                {

                    var stringifiedProperties = new Dictionary<string, string> ();

                    foreach (var property in properties)
                    {
                        if (property.Value != null)
                        {
                            if(property.Value is string stringValue)
                            {
                                stringifiedProperties[property.Key] = stringValue;
                            }

                            if (double.TryParse(property.Value.ToString(), out var numericValue))
                            {
                                stringifiedProperties[property.Key] = numericValue.ToString(CultureInfo.InvariantCulture);
                            }

                            else
                            {
                                stringifiedProperties[property.Key] = JsonConvert.SerializeObject(property.Value);
                            }
                        }
                    }
                    
                    TelemetryClient.TrackTrace(message, (SeverityLevel) logSeverity, stringifiedProperties);
                }
            }

            if (CurrentBuildConfig.Equals(BuildConfig.Debug))
            {
                ConsoleLogger.LogTrace(message, logSeverity, properties);
            }
        }   

        public void LogEvent(string eventName, Dictionary<string, object> properties = null, Dictionary<string, double> metrics = null)
        {
            if (properties == null)
            {
                properties = new Dictionary<string, object>();
            }

            var stringifiedProperties = new Dictionary<string, string>();

            foreach (var property in properties)
            {
                if (property.Value != null)
                {
                    if(property.Value is string stringValue)
                    {
                        stringifiedProperties[property.Key] = stringValue;
                    }

                    if (double.TryParse(property.Value.ToString(), out var numericValue))
                    {
                        stringifiedProperties[property.Key] = numericValue.ToString(CultureInfo.InvariantCulture);
                    }

                    else
                    {
                        stringifiedProperties[property.Key] = JsonConvert.SerializeObject(property.Value);
                    }
                }
            }
                
            TelemetryClient.TrackEvent(eventName, stringifiedProperties, metrics);

            if (CurrentBuildConfig.Equals(BuildConfig.Debug))
            {
                ConsoleLogger.LogEvent(eventName, properties, metrics);
            }
        }

        //todo next
        public void LogException(Exception exception, Dictionary<string, object> properties = null, Dictionary<string, double> metrics = null)
        {
            if (properties == null)
            {
                properties = new Dictionary<string, object>();
            }

            var stringifiedProperties = new Dictionary<string, string>();

            foreach (var property in properties)
            {
                if (property.Value != null)
                {
                    if(property.Value is string stringValue)
                    {
                        stringifiedProperties[property.Key] = stringValue;
                    }

                    if (double.TryParse(property.Value.ToString(), out var numericValue))
                    {
                        stringifiedProperties[property.Key] = numericValue.ToString(CultureInfo.InvariantCulture);
                    }

                    else
                    {
                        stringifiedProperties[property.Key] = JsonConvert.SerializeObject(property.Value);
                    }
                }
            }
                
            TelemetryClient.TrackException(exception, stringifiedProperties, metrics);

            if (CurrentBuildConfig.Equals(BuildConfig.Debug))
            {
                ConsoleLogger.LogException(exception, properties, metrics);
            }
        }
    }
}