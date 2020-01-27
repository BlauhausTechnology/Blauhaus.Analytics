using Blauhaus.Analytics.Abstractions.Config;
using Blauhaus.Analytics.Abstractions.Operation;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Analytics.Abstractions.TelemetryClients;
using Blauhaus.Analytics.Common.Service;
using Blauhaus.Analytics.Console.ConsoleLoggers;
using Blauhaus.Common.ValueObjects.BuildConfigs;
using Microsoft.ApplicationInsights.DataContracts;

namespace Blauhaus.Analytics.Server.Service
{
    public class AnalyticsServerServerService : BaseAnalyticsServerService, IAnalyticsServerService
    {

        public AnalyticsServerServerService(
            IApplicationInsightsConfig config, 
            IConsoleLogger appInsightsLogger, 
            ITelemetryClientProxy telemetryClient,
            IBuildConfig currentBuildConfig)
            : base(config, appInsightsLogger, telemetryClient, currentBuildConfig)
        {
        }

        public IAnalyticsOperation StartRequestOperation(string requestName, string operationName, string operationId, string sessionId)
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