using Blauhaus.AppInsights.Abstractions.Config;
using Blauhaus.Common.ValueObjects.BuildConfigs;
using Microsoft.ApplicationInsights.DataContracts;
using System.Collections.Generic;

namespace Blauhaus.AppInsights.Abstractions.ConsoleLoggers
{
    public class ConsoleLogger : IConsoleLogger
    {
        private readonly IApplicationInsightsConfig _config;
        private readonly ITraceProxy _traceProxy;
        private readonly IBuildConfig _currentBuildConfig;

        public ConsoleLogger(
            IApplicationInsightsConfig config,
            ITraceProxy traceProxy,
            IBuildConfig currentBuildConfig)
        {
            _config = config;
            _traceProxy = traceProxy;
            _currentBuildConfig = currentBuildConfig;
        }

        public void TrackEvent(string eventName, Dictionary<string, object> properties = null, Dictionary<string, double> metrics = null)
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

        public void TrackTrace(string message, SeverityLevel severityLevel = SeverityLevel.Verbose, Dictionary<string, object>? properties = null)
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
