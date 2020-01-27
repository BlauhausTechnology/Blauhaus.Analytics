using System.Collections.Generic;
using Blauhaus.AppInsights.Abstractions.Config;
using Blauhaus.AppInsights.Abstractions.Operation;
using Blauhaus.Common.ValueObjects.BuildConfigs;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

namespace Blauhaus.AppInsights.Abstractions.TelemetryClients
{
    public class TelemetryClientProxy : ITelemetryClientProxy
    {
        private readonly TelemetryClient _client;
        private readonly bool _isDebug;

        public TelemetryClientProxy(IApplicationInsightsConfig config, IBuildConfig buildConfig)
        {
            _client = new TelemetryClient(new TelemetryConfiguration(config.InstrumentationKey));
            _client.Context.Device.Type = config.ClientName;
            _isDebug = (BuildConfig) buildConfig == BuildConfig.Debug;
        }


        public ITelemetryClientProxy UpdateOperation(IAnalyticsOperation analyticsOperation, string sessiondId)
        {
            _client.Context.Operation.Id = analyticsOperation.Id;
            _client.Context.Operation.Name = analyticsOperation.Name;
            _client.Context.Session.Id = sessiondId;
            return this;
        }

        public void TrackDependency(DependencyTelemetry dependencyTelemetry)
        {
            _client.TrackDependency(dependencyTelemetry);
            if(_isDebug)_client.Flush();
        }

        public void TrackRequest(RequestTelemetry requestTelemetry)
        {
            _client.TrackRequest(requestTelemetry);
            if(_isDebug)_client.Flush();
        }

        public void TrackPageView(PageViewTelemetry pageViewTelemetry)
        {
            _client.TrackPageView(pageViewTelemetry);
            if(_isDebug)_client.Flush();
        }

        public void TrackTrace(string message, SeverityLevel severityLevel, Dictionary<string, string> properties)
        {
            _client.TrackTrace(message, severityLevel, properties);
            if(_isDebug)_client.Flush();
        }

        public void TrackEvent(string eventName, Dictionary<string, string> properties, Dictionary<string, double> metrics)
        {
            _client.TrackEvent(eventName, properties, metrics);
            if(_isDebug)_client.Flush();
        }
    }
}