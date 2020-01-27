using System;
using System.Collections.Generic;
using Blauhaus.AppInsights.Abstractions.Config;
using Blauhaus.AppInsights.Abstractions.Service;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Blauhaus.AppInsights.Abstractions.Operation;
using Blauhaus.AppInsights.Abstractions.ConsoleLoggers;
using Blauhaus.AppInsights.Abstractions.TelemetryClients;

namespace Blauhaus.AppInsights.Server.Service
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