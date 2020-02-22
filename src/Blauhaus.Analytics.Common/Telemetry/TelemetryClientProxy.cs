using System;
using System.Collections.Generic;
using Blauhaus.Analytics.Abstractions.Config;
using Blauhaus.Analytics.Abstractions.Operation;
using Blauhaus.Analytics.Abstractions.Session;
using Blauhaus.Common.ValueObjects.BuildConfigs;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

namespace Blauhaus.Analytics.Common.Telemetry
{
    public class TelemetryClientProxy : ITelemetryClientProxy
    {
        private readonly TelemetryClient _client;
        private readonly bool _isDebug;

        public TelemetryClientProxy(IApplicationInsightsConfig config, IBuildConfig buildConfig)
        {
            _client = new TelemetryClient(new TelemetryConfiguration(config.InstrumentationKey));
            _client.Context.Cloud.RoleName = config.RoleName;
            _isDebug = (BuildConfig) buildConfig == BuildConfig.Debug;
        }


        public void TrackException(Exception exception, Dictionary<string, string> properties, Dictionary<string, double> metrics)
        {
            _client.TrackException(exception, properties, metrics);
        }

        public void TrackTrace(TraceTelemetry traceTelemetry)
        {
            _client.TrackTrace(traceTelemetry);
        }

        public void TrackEvent(EventTelemetry eventTelemetry)
        {
            _client.TrackEvent(eventTelemetry);
        }

        public void TrackException(ExceptionTelemetry exceptionTelemetry)
        {
            _client.TrackException(exceptionTelemetry);
        }

        public void TrackDependency(DependencyTelemetry dependencyTelemetry)
        {
            _client.TrackDependency(dependencyTelemetry);
        }

        public void TrackRequest(RequestTelemetry requestTelemetry)
        {
            _client.TrackRequest(requestTelemetry);
        }

        public void TrackPageView(PageViewTelemetry pageViewTelemetry)
        {
            _client.TrackPageView(pageViewTelemetry);
        }


    }
}