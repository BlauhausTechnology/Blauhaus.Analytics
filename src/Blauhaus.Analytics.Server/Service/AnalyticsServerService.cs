using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using Blauhaus.Analytics.Abstractions.Config;
using Blauhaus.Analytics.Abstractions.Http;
using Blauhaus.Analytics.Abstractions.Operation;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Analytics.Abstractions.Session;
using Blauhaus.Analytics.Abstractions.TelemetryClients;
using Blauhaus.Analytics.Common.Service;
using Blauhaus.Analytics.Common.TelemetryClients;
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
            ITelemetryDecorator telemetryDecorator,
            IBuildConfig currentBuildConfig)
            : base(config, appInsightsLogger, telemetryClient, telemetryDecorator, currentBuildConfig)
        {
        }

        public IAnalyticsOperation StartRequestOperation(string requestName, string operationName, string operationId, IAnalyticsSession session)
        {
            CurrentSession = session;

            CurrentOperation = new AnalyticsOperation(operationName, operationId, duration =>
            {
                var requestTelemetry = new RequestTelemetry
                {
                    Duration = duration,
                    Name = requestName
                };
                
                TelemetryClient.TrackRequest(TelemetryDecorator.DecorateTelemetry(requestTelemetry, CurrentOperation, CurrentSession, 
                    new Dictionary<string, object>(), new Dictionary<string, double>()));

                ConsoleLogger.LogOperation(requestName, duration);

                CurrentOperation = null;
            });

            return CurrentOperation;
        }

        public IAnalyticsOperation StartRequestOperation(string requestName, IDictionary<string, string> headers)
        {
            if (!headers.TryGetValue(AnalyticsHeaders.Operation.Name, out var operationName))
                operationName = "NewRequest";

            if (!headers.TryGetValue(AnalyticsHeaders.Operation.Id, out var operationId))
                operationId = Guid.NewGuid().ToString();

            if (!headers.TryGetValue(AnalyticsHeaders.Session.Id, out var sessionId))
                sessionId = Guid.NewGuid().ToString();
            
            var session = AnalyticsSession.FromExisting(sessionId);

            if (headers.TryGetValue(AnalyticsHeaders.Session.AppVersion, out var appVersion))
                session.AppVersion = appVersion;

            if (headers.TryGetValue(AnalyticsHeaders.Session.AccountId, out var accountId))
                session.AccountId = accountId;

            if (headers.TryGetValue(AnalyticsHeaders.Session.DeviceId, out var deviceId))
                session.DeviceId = deviceId;
            
            if (headers.TryGetValue(AnalyticsHeaders.Session.UserId, out var userId))
                session.UserId = userId;

            foreach (var header in headers)
            {
                if (header.Key.StartsWith(AnalyticsHeaders.Prefix))
                {
                    var propertyName = header.Key.Substring(AnalyticsHeaders.Prefix.Length);
                    session.SetProperty(propertyName, header.Value);
                }
            }


            return StartRequestOperation(requestName, operationName, operationId, session);

        }
    }
}