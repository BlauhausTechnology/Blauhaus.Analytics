using System.Collections.Generic;
using Blauhaus.Analytics.Abstractions.Operation;
using Blauhaus.Analytics.Abstractions.Session;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;

namespace Blauhaus.Analytics.Common.TelemetryClients
{
    public interface ITelemetryDecorator
    {
        TTelemetry DecorateTelemetry<TTelemetry>(TTelemetry telemetry, IAnalyticsOperation currentOperation,
            IAnalyticsSession currentSession, Dictionary<string, object> properties) where TTelemetry : ITelemetry, ISupportProperties;
        
        TTelemetry DecorateTelemetry<TTelemetry>(TTelemetry telemetry, IAnalyticsOperation currentOperation,
            IAnalyticsSession currentSession, Dictionary<string, object> properties, Dictionary<string, double> metrics) 
            where TTelemetry : ITelemetry, ISupportProperties, ISupportMetrics;
    }
}