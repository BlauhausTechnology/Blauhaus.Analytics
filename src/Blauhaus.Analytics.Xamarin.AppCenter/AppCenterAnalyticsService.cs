using System;
using System.Collections.Generic;
using System.Linq;
using Blauhaus.Analytics.Abstractions.Config;
using Blauhaus.Analytics.Abstractions.Extensions;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Analytics.Abstractions.Session;
using Blauhaus.Analytics.Common.Service;
using Blauhaus.Analytics.Common.Telemetry;
using Blauhaus.Analytics.Console.ConsoleLoggers;
using Blauhaus.Common.ValueObjects.BuildConfigs;
using Blauhaus.Errors;
using Microsoft.AppCenter.Crashes;

namespace Blauhaus.Analytics.Xamarin.AppCenter
{
    public class AppCenterAnalyticsService : AnalyticsService
    {

        private string _userId = string.Empty;

        public AppCenterAnalyticsService(
            IApplicationInsightsConfig config, 
            IConsoleLogger consoleLogger, 
            ITelemetryClientProxy telemetryClient, 
            ITelemetryDecorator telemetryDecorator, 
            IBuildConfig currentBuildConfig, 
            IAnalyticsSessionFactory sessionFactory) 
                : base(config, consoleLogger, telemetryClient, telemetryDecorator, currentBuildConfig, sessionFactory)
        {
        }

        public override void LogEvent(object sender, string eventName, Dictionary<string, object> properties = null, string callerMemberName = "")
        { 
            Microsoft.AppCenter.Analytics.Analytics.TrackEvent(eventName, UpdateInformation(sender, properties, callerMemberName));
            
            base.LogEvent(sender, eventName, properties, callerMemberName);
        }

        public override void LogException(object sender, Exception exception, Dictionary<string, object> properties = null, string callerMemberName = "")
        {
            Crashes.TrackError(exception, UpdateInformation(sender, properties, callerMemberName));
            
            base.LogException(sender, exception, properties, callerMemberName);
        }

        private Dictionary<string, string> UpdateInformation(object sender, Dictionary<string, object> properties, string callerMemberName)
        {
            var props = properties.ToDictionaryOfStrings();
            
            if (CurrentSession.UserId != null && CurrentSession.UserId != _userId)
            {
                _userId = CurrentSession.UserId;
                Microsoft.AppCenter.AppCenter.SetUserId(CurrentSession.UserId);
            }
 
            props["SessionId"] = CurrentSession.Id;
            props["DeviceIdentifier"] = CurrentSession.DeviceId;
            props["UserId"] = CurrentSession.AccountId;
            props["AccountId"] = CurrentSession.AccountId;
            props["Class"] = sender.GetType().Name;
            props["Method"] = callerMemberName;

            if (CurrentOperation != null)
            {
                props["OperationName"] = CurrentOperation.Name;
                props["OperationId"] = CurrentOperation.Id;
            }

            return props;

        }

    }
}