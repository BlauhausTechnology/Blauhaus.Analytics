using System;
using Blauhaus.AppInsights.Abstractions.Config;
using Blauhaus.AppInsights.Abstractions.Service;
using Microsoft.ApplicationInsights.DataContracts;
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


        public IAnalyticsOperation StartPageViewOperation(string pageName)
        {
            CurrentOperation = new AnalyticsOperation(pageName, duration =>
            {
                var pageViewTelemetry = new PageViewTelemetry(pageName)
                {
                    Duration = duration
                };
                TelemetryClient.TrackPageView(pageViewTelemetry);
                CurrentOperation = null;
            });

            return CurrentOperation;
        }

    }
}