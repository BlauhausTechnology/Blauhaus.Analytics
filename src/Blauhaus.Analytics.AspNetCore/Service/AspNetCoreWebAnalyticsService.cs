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
    public class AspNetCoreWebAnalyticsService : BaseAspNetCoreAnalyticsService
    {
        protected readonly IHttpContextAccessor HttpContextAccessor;
        private IAnalyticsSession _currentSession;

        public AspNetCoreWebAnalyticsService(
            IApplicationInsightsConfig config, 
            IConsoleLogger appInsightsLogger, 
            ITelemetryClientProxy telemetryClient, 
            ITelemetryDecorator telemetryDecorator, 
            IBuildConfig currentBuildConfig, 
            IHttpContextAccessor httpContextAccessor) 
                : base(config, appInsightsLogger, telemetryClient, telemetryDecorator, currentBuildConfig)
        {
            HttpContextAccessor = httpContextAccessor;
        }

        protected override IAnalyticsSession GetSessionForCurrentUser()
        {

            if (_currentSession != null)
            {
                return _currentSession;
            }

            _currentSession = AnalyticsSession.New;

            if (HttpContextAccessor.HttpContext == null)
            {
                //there is no user
                return _currentSession;
            }


            var userId = HttpContextAccessor.HttpContext.Request.Cookies["UserId"];
            if (string.IsNullOrEmpty(userId))
            {
                userId = Guid.NewGuid().ToString();
                HttpContextAccessor.HttpContext.Response.Cookies.Append("UserId", userId, new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddYears(5)
                });
            }

            _currentSession.UserId = userId;
            
            return _currentSession;

        }


    }
}