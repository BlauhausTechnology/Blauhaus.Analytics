using Microsoft.ApplicationInsights;

namespace Blauhaus.AppInsights.Abstractions.Service
{
    public interface IAppInsightsService
    {
        TelemetryClient GetClient();
    }
}