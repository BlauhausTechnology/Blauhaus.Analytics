using Blauhaus.Analytics.Abstractions.Config;
using Blauhaus.Analytics.Abstractions.Operation;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Analytics.Abstractions.TelemetryClients;
using Blauhaus.Analytics.Common.Service;
using Blauhaus.Analytics.Console.ConsoleLoggers;
using Microsoft.ApplicationInsights.DataContracts;

namespace Blauhaus.Analytics.Server.Service
{
    public class AppInsightsServerService : BaseAppInsightsService, IAppInsightsServerService
    {

        public AppInsightsServerService(IApplicationInsightsConfig config, IConsoleLogger appInsightsLogger, ITelemetryClientProxy telemetryClient)
            : base(config, appInsightsLogger, telemetryClient)
        {
        }

        public IAnalyticsOperation StartRequestOperation(string requestName, string operationId, string operationName, string sessionId)
        {
            CurrentSessionId = sessionId;

            CurrentOperation = new AnalyticsOperation(operationId, operationName, duration =>
            {
                var requestTelemetry = new RequestTelemetry
                {
                    Duration = duration,
                    Name = requestName
                };
                TelemetryClient.TrackRequest(requestTelemetry);
                CurrentOperation = null;
            });

            return CurrentOperation;
        }

    }
}