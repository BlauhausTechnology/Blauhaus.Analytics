using System.Collections.Generic;
using Blauhaus.Analytics.Abstractions.Config;
using Blauhaus.Analytics.Abstractions.Operation;
using Blauhaus.Analytics.Abstractions.Session;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;

namespace Blauhaus.Analytics.Common.TelemetryClients
{
    public class TelemetryDecorator : ITelemetryDecorator
    {
        private readonly IApplicationInsightsConfig _config;

        public TelemetryDecorator(IApplicationInsightsConfig config)
        {
            _config = config;
        }

        public TTelemetry DecorateTelemetry<TTelemetry>(
            TTelemetry telemetry, 
            IAnalyticsOperation currentOperation, 
            IAnalyticsSession currentSession, 
            Dictionary<string, object> properties) 
                where TTelemetry : ITelemetry, ISupportProperties
        {

            telemetry.Context.Cloud.RoleName = _config.RoleName;
            telemetry.Context.InstrumentationKey = _config.InstrumentationKey;

            //todo tests vvv

            if (currentOperation != null)
            {
                telemetry.Context.Operation.Id = currentOperation.Id;
                telemetry.Context.Operation.Name = currentOperation.Name;
            }
            telemetry.Context.Session.Id = currentSession.Id;

            if (currentSession.AppVersion != null)
                telemetry.Context.Component.Version = currentSession.AppVersion;

            if (currentSession.AccountId != null)
                telemetry.Context.User.AccountId = currentSession.AccountId;

            if (currentSession.UserId != null)
                telemetry.Context.User.AuthenticatedUserId = currentSession.UserId;

            if (currentSession.DeviceId != null)
                telemetry.Context.Device.Id = currentSession.DeviceId;

            if (currentSession.Properties != null)
            {
                foreach (var sessionProperty in currentSession.Properties)
                {
                    telemetry.Properties[sessionProperty.Key] = sessionProperty.Value;
                }
            }

            foreach (var property in properties)
            {
                
                telemetry.Properties[property.Key] = property.Value.ToString();
            }

            return telemetry;
        }

        public TTelemetry DecorateTelemetry<TTelemetry>(TTelemetry telemetry, IAnalyticsOperation currentOperation, IAnalyticsSession currentSession, Dictionary<string, object> properties, Dictionary<string, double> metrics) where TTelemetry : ITelemetry, ISupportProperties, ISupportMetrics
        {
            telemetry = DecorateTelemetry(telemetry, currentOperation, currentSession, properties);

            //add metrics

            return telemetry;
        }
    }
}