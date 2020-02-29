using System;
using Blauhaus.Analytics.Abstractions.Config;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Analytics.Abstractions.Session;
using Blauhaus.Analytics.AspNetCore.Service._Base;
using Blauhaus.Analytics.Common.Service;
using Blauhaus.Analytics.Common.Telemetry;
using Blauhaus.Analytics.Console.ConsoleLoggers;
using Blauhaus.Common.ValueObjects.BuildConfigs;
using Microsoft.AspNetCore.Http;

namespace Blauhaus.Analytics.AspNetCore.Service
{
    public class AspNetCoreApiAnalyticsService : BaseAspNetCoreAnalyticsService
    {
        public AspNetCoreApiAnalyticsService(
            IApplicationInsightsConfig config, 
            IConsoleLogger appInsightsLogger, 
            ITelemetryClientProxy telemetryClient, 
            ITelemetryDecorator telemetryDecorator, 
            IBuildConfig currentBuildConfig, 
            IHttpContextAccessor httpContextAccessor) 
                : base(config, appInsightsLogger, telemetryClient, telemetryDecorator, currentBuildConfig)
        {
        }

        protected override IAnalyticsSession GetSessionForCurrentUser()
        {
            //Session will be populated from request headers.
            return AnalyticsSession.New;
        }
    }
}