using System.Collections.Generic;
using Blauhaus.AppInsights.Abstractions.Config;
using Blauhaus.AppInsights.Abstractions.Operation;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

namespace Blauhaus.AppInsights.Abstractions.TelemetryClients
{
    public class TelemetryClientProxy : ITelemetryClientProxy
    {
        private readonly TelemetryClient _client;

        public TelemetryClientProxy(IApplicationInsightsConfig config)
        {
            _client = new TelemetryClient(new TelemetryConfiguration(config.InstrumentationKey));
            _client.Context.Device.Type = config.ClientName;
        }


        public void Flush()
        {
            _client.Flush();
        }

        public void UpdateOperation(IAnalyticsOperation analyticsOperation, string sessiondId)
        {
            _client.Context.Operation.Id = analyticsOperation.Id;
            _client.Context.Operation.Name = analyticsOperation.Name;
            _client.Context.Session.Id = sessiondId;
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

        public void TrackTrace(string message, SeverityLevel severityLevel, Dictionary<string, string> properties)
        {
            _client.TrackTrace(message, severityLevel, properties);
        }

        public void TrackEvent(string eventName, Dictionary<string, string> properties, Dictionary<string, double> metrics)
        {
            _client.TrackEvent(eventName, properties, metrics);
        }
    }
}