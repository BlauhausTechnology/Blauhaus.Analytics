using System.Collections.Generic;
using Blauhaus.Analytics.Abstractions.Operation;
using Microsoft.ApplicationInsights.DataContracts;

namespace Blauhaus.Analytics.Abstractions.TelemetryClients
{
    public interface ITelemetryClientProxy 
    {
        ITelemetryClientProxy UpdateOperation(IAnalyticsOperation analyticsOperation, string sessiondId);
        
        void TrackTrace(string message, SeverityLevel severityLevel, Dictionary<string, string> properties);
        void TrackEvent(string eventName, Dictionary<string, string> properties, Dictionary<string, double> metrics);
        void TrackDependency(DependencyTelemetry dependencyTelemetry);
        void TrackRequest(RequestTelemetry requestTelemetry);
        void TrackPageView(PageViewTelemetry pageViewTelemetry);
    }
}