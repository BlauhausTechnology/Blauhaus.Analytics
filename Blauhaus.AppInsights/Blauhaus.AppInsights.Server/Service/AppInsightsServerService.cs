﻿using System;
using System.Collections.Generic;
using Blauhaus.AppInsights.Abstractions.Config;
using Blauhaus.AppInsights.Abstractions.Service;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

namespace Blauhaus.AppInsights.Server.Service
{
    public class AppInsightsServerService : BaseAppInsightsService, IAppInsightsServerService
    {
        private readonly TelemetryClient _telemetryClient;

        public AppInsightsServerService(IApplicationInsightsConfig config)
        {
            _telemetryClient = new TelemetryClient(new TelemetryConfiguration(config.InstrumentationKey));
            _telemetryClient.Context.Device.Type = "Server";
        }

        protected override TelemetryClient GetClient()
        {
            return _telemetryClient;
        }

        public AnalyticsOperation StartRequest(string requestName, string operationId, string operationName, string sessionId)
        {
            return new AnalyticsOperation(operationId, operationName, duration =>
            {
                var client = GetClient();
                client.Context.Operation.Id = operationId;
                client.Context.Operation.Name = operationName;
                client.Context.Session.Id = sessionId;

                var requestTelemetry = new RequestTelemetry()
                {
                    Duration = duration,
                    Name = requestName
                };
                client.TrackRequest(requestTelemetry);
                client.Flush();
                CurrentOperation = null;
            });
        }
    }
}