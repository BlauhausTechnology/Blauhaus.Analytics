using System.Collections.Generic;
using Blauhaus.Analytics.Abstractions.Config;
using Blauhaus.Analytics.Abstractions.Operation;
using Blauhaus.Analytics.Abstractions.Session;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Newtonsoft.Json;

namespace Blauhaus.Analytics.Common.Telemetry
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
            string className, 
            IAnalyticsOperation currentOperation, 
            IAnalyticsSession currentSession, 
            Dictionary<string, object> properties) 
                where TTelemetry : ITelemetry, ISupportProperties
        {

            telemetry.Context.Cloud.RoleName = _config.RoleName;
            telemetry.Context.Cloud.RoleInstance = className;
            telemetry.Context.InstrumentationKey = _config.InstrumentationKey;


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

            if (properties != null)
            {
                foreach (var property in properties)
                {
                    if (!property.Value.GetType().IsValueType && property.Value.GetType() != typeof(string))
                    {
                        telemetry.Properties[property.Key] = JsonConvert.SerializeObject(property.Value);
                    }
                    else
                    {
                        telemetry.Properties[property.Key] = property.Value.ToString();
                    }
                }
            }

            return telemetry;
        }

        public TTelemetry DecorateTelemetry<TTelemetry>(TTelemetry telemetry, string className, IAnalyticsOperation currentOperation, IAnalyticsSession currentSession, Dictionary<string, object> properties, Dictionary<string, double> metrics) where TTelemetry : ITelemetry, ISupportProperties, ISupportMetrics
        {
            telemetry = DecorateTelemetry(telemetry, className, currentOperation, currentSession, properties);

            if (metrics != null)
            {
                foreach (var metric in metrics)
                {
                    telemetry.Metrics[metric.Key] = metric.Value;
                }
            }

            return telemetry;
        }
    }
}