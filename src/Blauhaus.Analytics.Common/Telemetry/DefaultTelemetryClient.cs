using Microsoft.ApplicationInsights.DataContracts;

namespace Blauhaus.Analytics.Common.Telemetry;

public class DefaultTelemetryClient : ITelemetryClientProxy
{
    public void TrackTrace(TraceTelemetry traceTelemetry)
    {
    }

    public void TrackEvent(EventTelemetry eventTelemetry)
    {
    }

    public void TrackException(ExceptionTelemetry exceptionTelemetry)
    {
    }

    public void TrackDependency(DependencyTelemetry dependencyTelemetry)
    {
    }

    public void TrackRequest(RequestTelemetry requestTelemetry)
    {
    }

    public void TrackPageView(PageViewTelemetry pageViewTelemetry)
    {
    }
}