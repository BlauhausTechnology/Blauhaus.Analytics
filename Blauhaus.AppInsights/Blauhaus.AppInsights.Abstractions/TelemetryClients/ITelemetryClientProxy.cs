using System.Collections.Generic;
using Blauhaus.AppInsights.Abstractions.ConsoleLoggers;
using Blauhaus.AppInsights.Abstractions.Operation;
using Microsoft.ApplicationInsights.DataContracts;

namespace Blauhaus.AppInsights.Abstractions.TelemetryClients
{
    public interface ITelemetryClientProxy : IAppInsightsLogger
    {
        void Flush();
        void UpdateOperation(IAnalyticsOperation analyticsOperation, string sessiondId);

        void TrackDependency(DependencyTelemetry dependencyTelemetry);
        void TrackRequest(RequestTelemetry requestTelemetry);
        void TrackPageView(PageViewTelemetry pageViewTelemetry);
    }
}