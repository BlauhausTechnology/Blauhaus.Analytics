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

namespace Blauhaus.AppInsights.Client.Service
{
    public class AppInsightsClientService : BaseAppInsightsService, IAppInsightsClientService
    {
        public AppInsightsClientService(IApplicationInsightsConfig config, IConsoleLogger appInsightsLogger, ITelemetryClientProxy telemetryClient)
            : base(config, appInsightsLogger, telemetryClient)
        {
            var sessionId = Guid.NewGuid().ToString();
            CurrentSessionId = sessionId;
        }

        protected override TelemetryClient ConstructTelementryClient()
        {
            var client = new TelemetryClient(new TelemetryConfiguration(Config.InstrumentationKey));
            client.Context.Device.Type = "Client";
            return client;
        }

        public IAnalyticsOperation StartPageView(string pageName)
        {
            var operationName = "PageView." + pageName;

            CurrentOperation = new AnalyticsOperation(operationName, duration =>
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