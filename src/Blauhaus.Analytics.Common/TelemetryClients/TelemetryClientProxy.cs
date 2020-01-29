using System;
using System.Collections.Generic;
using Blauhaus.Analytics.Abstractions.Config;
using Blauhaus.Analytics.Abstractions.Operation;
using Blauhaus.Analytics.Abstractions.Session;
using Blauhaus.Analytics.Abstractions.TelemetryClients;
using Blauhaus.Common.ValueObjects.BuildConfigs;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

namespace Blauhaus.AppInsights.Abstractions.TelemetryClients
{
    public class TelemetryClientProxy : ITelemetryClientProxy
    {
        private readonly TelemetryClient _client;
        private readonly bool _isDebug;

        public TelemetryClientProxy(IApplicationInsightsConfig config, IBuildConfig buildConfig)
        {
            _client = new TelemetryClient(new TelemetryConfiguration(config.InstrumentationKey));
            _client.Context.Device.Type = config.ClientName;
            _isDebug = (BuildConfig) buildConfig == BuildConfig.Debug;
        }


        public ITelemetryClientProxy UpdateOperation(IAnalyticsOperation analyticsOperation, IAnalyticsSession session)
        {
            if (analyticsOperation != null)
            {
                _client.Context.Operation.Id = analyticsOperation.Id;
                _client.Context.Operation.Name = analyticsOperation.Name;
            }
            _client.Context.Session.Id = session.Id;

            if (session.AppVersion != null)
                _client.Context.Component.Version = session.AppVersion;

            if (session.AccountId != null)
                _client.Context.User.AccountId = session.AccountId;

            if (session.UserId != null)
                _client.Context.User.AuthenticatedUserId = session.UserId;

            if (session.DeviceId != null)
                _client.Context.Device.Id = session.DeviceId;

            foreach (var sessionProperty in session.Properties)
            {
                _client.Context.GlobalProperties[sessionProperty.Key] = sessionProperty.Value;
            }

            return this;
        }

        public void TrackException(Exception exception, Dictionary<string, string> properties, Dictionary<string, double> metrics)
        {
            _client.TrackException(exception, properties, metrics);
            if(_isDebug)_client.Flush();
        }

        public void TrackDependency(DependencyTelemetry dependencyTelemetry)
        {
            _client.TrackDependency(dependencyTelemetry);
            if(_isDebug)_client.Flush();
        }

        public void TrackRequest(RequestTelemetry requestTelemetry)
        {
            _client.TrackRequest(requestTelemetry);
            if(_isDebug)_client.Flush();
        }

        public void TrackPageView(PageViewTelemetry pageViewTelemetry)
        {
            _client.TrackPageView(pageViewTelemetry);
            if(_isDebug)_client.Flush();
        }

        public void TrackTrace(string message, SeverityLevel severityLevel, Dictionary<string, string> properties)
        {
            _client.TrackTrace(message, severityLevel, properties);
            if(_isDebug)_client.Flush();
        }

        public void TrackEvent(string eventName, Dictionary<string, string> properties, Dictionary<string, double> metrics)
        {
            _client.TrackEvent(eventName, properties, metrics);
            if(_isDebug)_client.Flush();
        }
    }
}