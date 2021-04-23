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
        private string _accountId = string.Empty;

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
            Microsoft.AppCenter.Analytics.Analytics.TrackEvent(eventName, UpdateInformation(properties));
            
            base.LogEvent(sender, eventName, properties, callerMemberName);
        }

        public override void LogException(object sender, Exception exception, Dictionary<string, object> properties = null, string callerMemberName = "")
        {
            Crashes.TrackError(exception, UpdateInformation(properties));
            
            base.LogException(sender, exception, properties, callerMemberName);
        }

        private Dictionary<string, string> UpdateInformation(Dictionary<string, object> properties)
        {
            var props = properties.ToDictionaryOfStrings();
            
            if (CurrentSession.UserId != null && CurrentSession.UserId != _userId)
            {
                _userId = CurrentSession.UserId;
                Microsoft.AppCenter.AppCenter.SetUserId(CurrentSession.UserId);
            }

            if (CurrentSession.AccountId != null && CurrentSession.AccountId != _accountId)
            {
                _accountId = CurrentSession.AccountId;
                props["AccountId"] = CurrentSession.AccountId;
            }

            if (CurrentOperation != null)
            {
                props["OperationName"] = CurrentOperation.Name;
                props["OperationId"] = CurrentOperation.Id;
            }

            return props;

        }

    }
}