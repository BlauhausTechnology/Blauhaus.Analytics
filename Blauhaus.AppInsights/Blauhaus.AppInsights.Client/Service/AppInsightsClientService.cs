using System;
using System.Collections.Generic;
using Blauhaus.AppInsights.Abstractions.Config;
using Blauhaus.AppInsights.Abstractions.Service;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Blauhaus.AppInsights.Abstractions.Operation;
using Blauhaus.AppInsights.Abstractions.ConsoleLoggers;

namespace Blauhaus.AppInsights.Client.Service
{
    public class AppInsightsClientService : BaseAppInsightsService, IAppInsightsClientService
    {
        private readonly TelemetryClient _telemetryClient;

        public AppInsightsClientService(IApplicationInsightsConfig config, IConsoleLogger consoleLogger)
            : base(config, consoleLogger)
        {
            var sessionId = Guid.NewGuid().ToString();
            _telemetryClient = new TelemetryClient(new TelemetryConfiguration(config.InstrumentationKey));
            _telemetryClient.Context.Session.Id = sessionId;
            _telemetryClient.Context.Device.Type = "Client";
            CurrentSessionId = sessionId;
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


        public IAnalyticsOperation StartPageView(string pageName)
        {
            var operationId = Guid.NewGuid().ToString();
            var operationName = "PageView." + pageName;

            CurrentOperation = new AnalyticsOperation(operationId, operationName, duration =>
            {
                var client = GetClient();

                var pageViewTelemetry = new PageViewTelemetry(operationName)
                {
                    Duration = duration
                };
                client.TrackPageView(pageViewTelemetry);
                client.Flush();
                CurrentOperation = null;
            });

            return CurrentOperation;
        }


    }
}