using System;
using System.Collections.Generic;
using Blauhaus.AppInsights.Abstractions.Config;
using Blauhaus.AppInsights.Abstractions.Service;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Blauhaus.AppInsights.Abstractions.Operation;
using Blauhaus.AppInsights.Abstractions.ConsoleLoggers;

namespace Blauhaus.AppInsights.Server.Service
{
    public class AppInsightsServerService : BaseAppInsightsService, IAppInsightsServerService
    {
        private readonly TelemetryClient _telemetryClient;

        public AppInsightsServerService(IApplicationInsightsConfig config, IConsoleLogger consoleLogger)
            : base(config, consoleLogger)
        {
            _telemetryClient = new TelemetryClient(new TelemetryConfiguration(config.InstrumentationKey));
            _telemetryClient.Context.Device.Type = "Server";
        }

        protected override TelemetryClient GetClient()
        {            
            if (CurrentOperation != null)
            {
                _telemetryClient.Context.Operation.Id = CurrentOperation.Id;
                _telemetryClient.Context.Operation.Name = CurrentOperation.Name;
            }

            return _telemetryClient;
        }

        public IAnalyticsOperation StartRequest(string requestName, string operationId, string operationName, string sessionId)
        {
            CurrentSessionId = sessionId;

            CurrentOperation = new AnalyticsOperation(operationId, operationName, duration =>
            {
                var client = GetClient();
                client.Context.Operation.Id = operationId;
                client.Context.Operation.Name = operationName;
                client.Context.Session.Id = CurrentSessionId;

                var requestTelemetry = new RequestTelemetry()
                {
                    Duration = duration,
                    Name = requestName
                };
                client.TrackRequest(requestTelemetry);
                client.Flush();
                CurrentOperation = null;
            });

            return CurrentOperation;
        }
    }
}