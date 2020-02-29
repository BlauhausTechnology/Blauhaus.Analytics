using System;
using Blauhaus.Analytics.Abstractions.Config;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Analytics.Abstractions.Session;
using Blauhaus.Analytics.Common.Service;
using Blauhaus.Analytics.Common.Telemetry;
using Blauhaus.Analytics.Console.ConsoleLoggers;
using Blauhaus.Common.ValueObjects.BuildConfigs;
using Microsoft.AspNetCore.Http;

namespace Blauhaus.Analytics.AspNetCore.Service
{
    public class AspNetCoreAnalyticsService : AnalyticsService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AspNetCoreAnalyticsService(
            IApplicationInsightsConfig config,
            IConsoleLogger appInsightsLogger,
            ITelemetryClientProxy telemetryClient,
            ITelemetryDecorator telemetryDecorator,
            IBuildConfig currentBuildConfig,
            IHttpContextAccessor httpContextAccessor)
            : base(config, appInsightsLogger, telemetryClient, telemetryDecorator, currentBuildConfig)
        {
            _httpContextAccessor = httpContextAccessor;

            //this is registered with scoped lifetime so each request gets a new one

            //todo
            //CurrentSession.AppVersion = applicationInfoService.CurrentVersion;
            //CurrentSession.DeviceId = deviceInfoService.DeviceUniqueIdentifier;
        }

        private IAnalyticsSession? _sessionForCurrentUser;

        public override IAnalyticsSession CurrentSession
        {
            get
            {
                if (_sessionForCurrentUser == null)
                {
                    _sessionForCurrentUser = AnalyticsSession.New;

                    if (_httpContextAccessor.HttpContext != null)
                    {
                        var userId = _httpContextAccessor.HttpContext.Request.Cookies["UserId"];
                        if (string.IsNullOrEmpty(userId))
                        {
                            userId = Guid.NewGuid().ToString();
                            _httpContextAccessor.HttpContext.Response.Cookies.Append("UserId", userId, new CookieOptions
                            {
                                Expires = DateTimeOffset.UtcNow.AddYears(5)
                            });
                        }

                        _sessionForCurrentUser.UserId = userId;
                    }
                   
                }

                return _sessionForCurrentUser;
            }
        }
    }
}