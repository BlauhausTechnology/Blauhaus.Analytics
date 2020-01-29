using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Blauhaus.Analytics.Abstractions.Config;
using Blauhaus.Analytics.Abstractions.Http;
using Blauhaus.Analytics.Abstractions.Operation;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Analytics.Abstractions.Session;
using Blauhaus.Analytics.Abstractions.TelemetryClients;
using Blauhaus.Analytics.Common.Service;
using Blauhaus.Analytics.Console.ConsoleLoggers;
using Blauhaus.Common.ValueObjects.BuildConfigs;
using Blauhaus.DeviceServices.Abstractions.Application;
using Blauhaus.DeviceServices.Abstractions.DeviceInfo;
using Microsoft.ApplicationInsights.DataContracts;

namespace Blauhaus.Analytics.Client.Service
{
    public class AnalyticsClientService : BaseAnalyticsService, IAnalyticsClientService
    {
        public AnalyticsClientService(
            IApplicationInsightsConfig config, 
            IConsoleLogger appInsightsLogger, 
            ITelemetryClientProxy telemetryClient,
            IBuildConfig currentBuildConfig,
            IDeviceInfoService deviceInfoService,
            IApplicationInfoService applicationInfoService)
            : base(config, appInsightsLogger, telemetryClient, currentBuildConfig)
        {
            CurrentSession = AnalyticsSession.New;
            CurrentSession.AppVersion = applicationInfoService.CurrentVersion;
            CurrentSession.DeviceId = deviceInfoService.DeviceUniqueIdentifier;
        }


        public IAnalyticsOperation StartPageViewOperation(string pageName)
        {
            CurrentOperation = new AnalyticsOperation(pageName, duration =>
            {
                var pageViewTelemetry = new PageViewTelemetry(pageName)
                {
                    Duration = duration
                };

                TelemetryClient.TrackPageView(pageViewTelemetry);
                ConsoleLogger.LogOperation(pageName, duration);
                
                CurrentOperation = null;
            });

            return CurrentOperation;
        }

        private readonly Dictionary<string, string> _analyticsOperationHeaders = new Dictionary<string, string>();
        public IDictionary<string, string> AnalyticsOperationHeaders
        {
            get
            {
                _analyticsOperationHeaders[AnalyticsHeaders.Operation.Name] = CurrentOperation?.Name;
                _analyticsOperationHeaders[AnalyticsHeaders.Operation.Id] = CurrentOperation?.Id;

                if (CurrentSession != null)
                {
                    _analyticsOperationHeaders[AnalyticsHeaders.Session.Id] = CurrentSession.Id;
                    
                    if(CurrentSession.AccountId != null)
                        _analyticsOperationHeaders[AnalyticsHeaders.Session.AccountId] = CurrentSession.AccountId;
                    
                    if(CurrentSession.UserId != null)
                        _analyticsOperationHeaders[AnalyticsHeaders.Session.UserId] = CurrentSession.UserId;
                    
                    if(CurrentSession.DeviceId != null)
                        _analyticsOperationHeaders[AnalyticsHeaders.Session.DeviceId] = CurrentSession.DeviceId;
                    
                    if(CurrentSession.AppVersion != null)
                        _analyticsOperationHeaders[AnalyticsHeaders.Session.AppVersion] = CurrentSession.AppVersion;
                
                    foreach (var currentSessionProperty in CurrentSession.Properties)
                    {
                        _analyticsOperationHeaders[AnalyticsHeaders.Prefix + currentSessionProperty.Key] = currentSessionProperty.Value;
                    }
                }
                
                return _analyticsOperationHeaders;
            }
        }

        public void ClearCurrentSession()
        {
            CurrentOperation?.Dispose();
            CurrentOperation = null;
            var sessionId = CurrentSession.Id;
            CurrentSession = AnalyticsSession.FromExisting(sessionId);
        }
    }
}