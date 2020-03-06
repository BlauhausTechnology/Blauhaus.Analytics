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

        public TTelemetry DecorateTelemetry<TTelemetry>(TTelemetry telemetry,
            string className,
            string callerMemberName,
            IAnalyticsOperation currentOperation,
            IAnalyticsSession currentSession,
            Dictionary<string, object> properties) 
                where TTelemetry : ITelemetry, ISupportProperties
        {

            telemetry.Context.Cloud.RoleName = _config.RoleName;
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

            if (currentSession.DeviceType != null)
                telemetry.Context.Device.Type = currentSession.DeviceType.Value;

            if (currentSession.Platform != null)
                telemetry.Context.Device.OperatingSystem = currentSession.Platform.Value;

            if (currentSession.OperatingSystemVersion != null)
                telemetry.Properties["OperatingSystemVersion"] = currentSession.OperatingSystemVersion;

            if (currentSession.Manufacturer != null)
                telemetry.Context.Device.OemName = currentSession.Manufacturer;

            if (currentSession.Model != null)
                telemetry.Context.Device.Model = currentSession.Model;

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

            telemetry.Properties["Class"] = className;
            telemetry.Properties["Method"] = callerMemberName;

            return telemetry;
        }

        public TTelemetry DecorateTelemetry<TTelemetry>(TTelemetry telemetry, string className, string memberName, IAnalyticsOperation currentOperation, IAnalyticsSession currentSession, Dictionary<string, object> properties, Dictionary<string, double> metrics) where TTelemetry : ITelemetry, ISupportProperties, ISupportMetrics
        {
            telemetry = DecorateTelemetry(telemetry, className, memberName, currentOperation, currentSession, properties);

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