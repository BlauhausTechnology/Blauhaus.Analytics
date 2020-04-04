using System;
using System.Collections.Generic;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Common.ValueObjects.BuildConfigs;

namespace Blauhaus.Analytics.Console.ConsoleLoggers
{
    public class ConsoleLogger : IConsoleLogger
    {
        private readonly ITraceProxy _traceProxy;
        private readonly IBuildConfig _currentBuildConfig;

        private bool ShouldLog() => _currentBuildConfig != null && _currentBuildConfig.Value != BuildConfig.Release.Value;

        public ConsoleLogger(
            ITraceProxy traceProxy,
            IBuildConfig currentBuildConfig)
        {
            _traceProxy = traceProxy;
            _currentBuildConfig = currentBuildConfig;
        }

        public void LogEvent(string eventName, Dictionary<string, string>? properties = null)
        {

            if (!ShouldLog())
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

        }

        public void LogException(Exception exception, Dictionary<string, string>? properties)
        {

            if (!ShouldLog())
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

        }

        public void LogOperation(string operationName, TimeSpan duration)
        {
            if (!ShouldLog())
            {
                return;
            }

            _traceProxy.SetColour(ConsoleColours.OperationColour);
            _traceProxy.Write(""); //newline per operation
            _traceProxy.Write($"OPERATION: {operationName} completed in {Math.Round((double) duration.Milliseconds)} ms");
        }

        public void LogTrace(string message, LogSeverity severityLevel = LogSeverity.Verbose, Dictionary<string, string>? properties = null)
        {
            if (!ShouldLog())
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
