using System;
using System.Collections.Generic;
using Blauhaus.Analytics.Abstractions.Operation;
using Blauhaus.Analytics.Abstractions.Session;
using Microsoft.ApplicationInsights.DataContracts;

namespace Blauhaus.Analytics.Abstractions.TelemetryClients
{
    public interface ITelemetryClientProxy 
    {
        ITelemetryClientProxy UpdateOperation(IAnalyticsOperation analyticsOperation, AnalyticsSession sessiond);
        
        void TrackTrace(string message, SeverityLevel severityLevel, Dictionary<string, string> properties);
        void TrackEvent(string eventName, Dictionary<string, string> properties, Dictionary<string, double> metrics);
        void TrackException(Exception exception, Dictionary<string, string> properties, Dictionary<string, double> metrics);
        void TrackDependency(DependencyTelemetry dependencyTelemetry);
        void TrackRequest(RequestTelemetry requestTelemetry);
        void TrackPageView(PageViewTelemetry pageViewTelemetry);
    }
}