﻿using System;
using Blauhaus.Analytics.Abstractions.Config;
using Blauhaus.Analytics.Abstractions.Operation;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Analytics.Abstractions.TelemetryClients;
using Blauhaus.Analytics.Common.Service;
using Blauhaus.Analytics.Console.ConsoleLoggers;
using Microsoft.ApplicationInsights.DataContracts;

namespace Blauhaus.Analytics.Client.Service
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