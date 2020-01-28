using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using Blauhaus.Analytics.Abstractions.Config;
using Blauhaus.Analytics.Abstractions.Http;
using Blauhaus.Analytics.Abstractions.Operation;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Analytics.Abstractions.TelemetryClients;
using Blauhaus.Analytics.Common.Service;
using Blauhaus.Analytics.Console.ConsoleLoggers;
using Blauhaus.Common.ValueObjects.BuildConfigs;
using Microsoft.ApplicationInsights.DataContracts;

namespace Blauhaus.Analytics.Server.Service
{
    public class AnalyticsServerService : BaseAnalyticsService, IAnalyticsServerService
    {

        public AnalyticsServerService(
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

            CurrentOperation = new AnalyticsOperation(operationName, operationId, duration =>
            {
                var requestTelemetry = new RequestTelemetry
                {
                    Duration = duration,
                    Name = requestName
                };
                
                TelemetryClient.TrackRequest(requestTelemetry);
                ConsoleLogger.LogOperation(requestName, duration);

                CurrentOperation = null;
            });

            return CurrentOperation;
        }

        public IAnalyticsOperation StartRequestOperation(string requestName, IDictionary<string, string> headers)
        {
            if (!headers.TryGetValue(AnalyticsHeaders.OperationName, out var operationNames))
            {
                throw new ArgumentException(AnalyticsHeaders.OperationName + " missing from request headers");
            }
            
            if (!headers.TryGetValue(AnalyticsHeaders.OperationId, out var operationIds))
            {
                throw new ArgumentException(AnalyticsHeaders.OperationId + " missing from request headers");
            }
            
            if (!headers.TryGetValue(AnalyticsHeaders.SessionId, out var sessionId))
            {
                throw new ArgumentException(AnalyticsHeaders.SessionId + " missing from request headers");
            }

            return StartRequestOperation(requestName, operationNames, operationIds, sessionId);

        }
    }
}