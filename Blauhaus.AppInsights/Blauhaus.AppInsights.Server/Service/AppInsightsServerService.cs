using Blauhaus.AppInsights.Abstractions.Service;
using Microsoft.ApplicationInsights;

namespace Blauhaus.AppInsights.Server.Service
{
    public class AppInsightsServerService : IAppInsightsService
    {
        private readonly TelemetryClient _telemetryClient;

        public AppInsightsServerService(TelemetryClient telemetryClient)
        {
            _telemetryClient = telemetryClient;
        }

        public TelemetryClient GetClient()
        {
            return _telemetryClient;
        }
    }
}