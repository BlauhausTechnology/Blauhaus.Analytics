using Blauhaus.AppInsights.Abstractions.Config;
using Blauhaus.AppInsights.Abstractions.Service;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;

namespace Blauhaus.AppInsights.Client.Service
{
    public class AppInsightsClientService : IAppInsightsService
    {
        private readonly IApplicationInsightsConfig _config;
        private TelemetryClient _telemetryClient;

        public AppInsightsClientService(IApplicationInsightsConfig config)
        {
            _config = config;
        }

        public TelemetryClient GetClient()
        {
            return _telemetryClient ??= new TelemetryClient(new TelemetryConfiguration(_config.InstrumentationKey));
        }
    }
}