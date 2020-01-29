using System;
using System.Collections.Generic;
using Blauhaus.Analytics.Abstractions.Config;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Common.ValueObjects.BuildConfigs;

namespace Blauhaus.Analytics.Console.ConsoleLoggers
{
    public class ConsoleLogger : IConsoleLogger
    {
        private readonly ITraceProxy _traceProxy;
        private readonly IBuildConfig _currentBuildConfig;

        public ConsoleLogger(
            ITraceProxy traceProxy,
            IBuildConfig currentBuildConfig)
        {
            _traceProxy = traceProxy;
            _currentBuildConfig = currentBuildConfig;
        }

        public void LogEvent(string eventName, Dictionary<string, object>? properties = null, Dictionary<string, double>? metrics = null)
        {

            if (_currentBuildConfig.Value == BuildConfig.Release.Value)
            {
                return;
            }

            _traceProxy.SetColour(ConsoleColours.EventColour);

            _traceProxy.Write($"EVENT: {eventName}");

            if (properties != null)
            {
                foreach (var property in properties)
                {
                    _traceProxy.Write($" * {property.Key}: {property.Value}");
                }
            }

            if (metrics != null)
            {
                foreach (var metric in metrics)
                {
                    _traceProxy.Write($" + {metric.Key}: {metric.Value}");
                }
            }


        }

        public void LogException(Exception exception, Dictionary<string, object>? properties, Dictionary<string, double>? metrics)
        {
            if (_currentBuildConfig.Value == BuildConfig.Release.Value)
            {
                return;
            }

            _traceProxy.SetColour(ConsoleColours.ExceptionColour);
            
            _traceProxy.Write(""); //newline per exception
            _traceProxy.Write($"EXCEPTION: {exception.Message}");
            _traceProxy.Write($"STACKTRACE {exception.StackTrace}");



            if (properties != null)
            {
                foreach (var property in properties)
                {
                    _traceProxy.Write($" !*! {property.Key}: {property.Value}");
                }
            }

            if (metrics != null)
            {
                foreach (var metric in metrics)
                {
                    _traceProxy.Write($" !+! {metric.Key}: {metric.Value}");
                }
            }

        }

        public void LogOperation(string operationName, TimeSpan duration)
        {
            if (_currentBuildConfig.Value == BuildConfig.Release.Value)
            {
                return;
            }

            _traceProxy.SetColour(ConsoleColours.OperationColour);
            _traceProxy.Write(""); //newline per operation
            _traceProxy.Write($"OPERATION: {operationName} completed in {Math.Round((double) duration.Milliseconds)} ms");
        }

        public void LogTrace(string message, LogSeverity severityLevel = LogSeverity.Verbose, Dictionary<string, object>? properties = null)
        {

            if (_currentBuildConfig.Value == BuildConfig.Release.Value)
            {
                return;
            }

            _traceProxy.SetColour(ConsoleColours.TraceColours[severityLevel]);
            _traceProxy.Write($"TRACE: {message}");

            if (properties != null)
            {
                foreach (var property in properties)
                {
                    _traceProxy.Write($" * {property.Key}: {property.Value}");
                }
            }

        }

    }

}
