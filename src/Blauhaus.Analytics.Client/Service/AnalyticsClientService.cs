using System;
using Blauhaus.Analytics.Abstractions.Config;
using Blauhaus.Analytics.Abstractions.Operation;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Analytics.Abstractions.TelemetryClients;
using Blauhaus.Analytics.Common.Service;
using Blauhaus.Analytics.Console.ConsoleLoggers;
using Blauhaus.Common.ValueObjects.BuildConfigs;
using Microsoft.ApplicationInsights.DataContracts;

namespace Blauhaus.Analytics.Client.Service
{
    public class AnalyticsClientService : BaseAnalyticsServerService, IAnalyticsClientService
    {
        public AnalyticsClientService(
            IApplicationInsightsConfig config, 
            IConsoleLogger appInsightsLogger, 
            ITelemetryClientProxy telemetryClient,
            IBuildConfig currentBuildConfig)
            : base(config, appInsightsLogger, telemetryClient, currentBuildConfig)
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