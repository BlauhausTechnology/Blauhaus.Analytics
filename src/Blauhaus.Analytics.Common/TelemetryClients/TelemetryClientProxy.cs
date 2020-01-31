using System;
using System.Collections.Generic;
using Blauhaus.Analytics.Abstractions.Config;
using Blauhaus.Analytics.Abstractions.Operation;
using Blauhaus.Analytics.Abstractions.Session;
using Blauhaus.Analytics.Abstractions.TelemetryClients;
using Blauhaus.Common.ValueObjects.BuildConfigs;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

namespace Blauhaus.AppInsights.Abstractions.TelemetryClients
{
    public class TelemetryClientProxy : ITelemetryClientProxy
    {
        private readonly IApplicationInsightsConfig _config;
        private readonly TelemetryClient _client;
        private readonly bool _isDebug;
        private IAnalyticsOperation _currentOperation;
        private IAnalyticsSession _currentSession;

        public TelemetryClientProxy(IApplicationInsightsConfig config, IBuildConfig buildConfig)
        {
            _config = config;
            _client = new TelemetryClient(new TelemetryConfiguration(config.InstrumentationKey));
            _client.Context.Cloud.RoleName = config.RoleName;
            _isDebug = (BuildConfig) buildConfig == BuildConfig.Debug;
        }


        public ITelemetryClientProxy UpdateOperation(IAnalyticsOperation analyticsOperation, IAnalyticsSession session)
        {
            _currentOperation = analyticsOperation;
            _currentSession = session;

            return this;
        }

        public void TrackException(Exception exception, Dictionary<string, string> properties, Dictionary<string, double> metrics)
        {
            _client.TrackException(exception, properties, metrics);
            if(_isDebug)_client.Flush();
        }

        public void TrackTrace(TraceTelemetry traceTelemetry)
        {
            throw new NotImplementedException();
        }

        public void TrackEvent(EventTelemetry eventTelemetry)
        {
            throw new NotImplementedException();
        }

        public void TrackException(ExceptionTelemetry exceptionTelemetry)
        {
            throw new NotImplementedException();
        }

        public void TrackDependency(DependencyTelemetry dependencyTelemetry)
        {
            DecorateTelemetry(dependencyTelemetry);
            _client.TrackDependency(dependencyTelemetry);
            if(_isDebug)_client.Flush();
        }

        public void TrackRequest(RequestTelemetry requestTelemetry)
        {
            DecorateTelemetry(requestTelemetry);
            _client.TrackRequest(requestTelemetry);
            if(_isDebug)_client.Flush();
        }

        public void TrackPageView(PageViewTelemetry pageViewTelemetry)
        {
            DecorateTelemetry(pageViewTelemetry);
            _client.TrackPageView(pageViewTelemetry);
            if(_isDebug)_client.Flush();
        }

        public void TrackTrace(string message, SeverityLevel severityLevel, Dictionary<string, string> properties)
        {
            var telemetry = new TraceTelemetry(message, severityLevel);
            _client.TrackTrace(message, severityLevel, properties);
            if(_isDebug)_client.Flush();
        }

        public void TrackEvent(string eventName, Dictionary<string, string> properties, Dictionary<string, double> metrics)
        {
            _client.TrackEvent(eventName, properties, metrics);
            if(_isDebug)_client.Flush();
        }


        private ITelemetry DecorateTelemetry(ITelemetry telemetry)
        {
            telemetry.Context.Cloud.RoleName = _config.RoleName;
            telemetry.Context.InstrumentationKey = _config.InstrumentationKey;

            if (_currentOperation != null)
            {
                telemetry.Context.Operation.Id = _currentOperation.Id;
                telemetry.Context.Operation.Name = _currentOperation.Name;
            }
            telemetry.Context.Session.Id = _currentSession.Id;

            if (_currentSession.AppVersion != null)
                telemetry.Context.Component.Version = _currentSession.AppVersion;

            if (_currentSession.AccountId != null)
                telemetry.Context.User.AccountId = _currentSession.AccountId;

            if (_currentSession.UserId != null)
                telemetry.Context.User.AuthenticatedUserId = _currentSession.UserId;

            if (_currentSession.DeviceId != null)
                telemetry.Context.Device.Id = _currentSession.DeviceId;

            foreach (var sessionProperty in _currentSession.Properties)
            {
                telemetry.Context.GlobalProperties[sessionProperty.Key] = sessionProperty.Value;
            }

            return telemetry;
        }
    }
}