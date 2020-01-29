using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Blauhaus.Analytics.Abstractions.Config;
using Blauhaus.Analytics.Abstractions.Http;
using Blauhaus.Analytics.Abstractions.Operation;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Analytics.Abstractions.Session;
using Blauhaus.Analytics.Abstractions.TelemetryClients;
using Blauhaus.Analytics.Common.Service;
using Blauhaus.Analytics.Console.ConsoleLoggers;
using Blauhaus.Common.ValueObjects.BuildConfigs;
using Microsoft.ApplicationInsights.DataContracts;

namespace Blauhaus.Analytics.Client.Service
{
    public class AnalyticsClientService : BaseAnalyticsService, IAnalyticsClientService
    {

        public AnalyticsClientService(
            IApplicationInsightsConfig config, 
            IConsoleLogger appInsightsLogger, 
            ITelemetryClientProxy telemetryClient,
            IBuildConfig currentBuildConfig)
            : base(config, appInsightsLogger, telemetryClient, currentBuildConfig)
        {
            CurrentSession = AnalyticsSession.New;
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
                ConsoleLogger.LogOperation(pageName, duration);
                
                CurrentOperation = null;
            });

            return CurrentOperation;
        }

        public IDictionary<string, string> AnalyticsOperationHeaders => new Dictionary<string, string>
        {
            {AnalyticsHeaders.OperationName, CurrentOperation?.Name },
            {AnalyticsHeaders.OperationId, CurrentOperation?.Id },
            {AnalyticsHeaders.SessionId, CurrentSession.Id }
        };

    }
}