using System;
using Blauhaus.Analytics.Abstractions.Config;
using Blauhaus.Analytics.Abstractions.Session;
using Blauhaus.Analytics.Common.Service;
using Blauhaus.Analytics.Common.Telemetry;
using Blauhaus.Analytics.Console.ConsoleLoggers;
using Blauhaus.Common.ValueObjects.BuildConfigs;
using Microsoft.AspNetCore.Http;

namespace Blauhaus.Analytics.AspNetCore.Service._Base
{
    public abstract class BaseAspNetCoreAnalyticsService : AnalyticsService
    {
        protected BaseAspNetCoreAnalyticsService(
            IApplicationInsightsConfig config,
            IConsoleLogger appInsightsLogger,
            ITelemetryClientProxy telemetryClient,
            ITelemetryDecorator telemetryDecorator,
            IBuildConfig currentBuildConfig)
            : base(config, appInsightsLogger, telemetryClient, telemetryDecorator, currentBuildConfig)
        {
        }

        private IAnalyticsSession? _sessionForCurrentUser;

        public override IAnalyticsSession CurrentSession => _sessionForCurrentUser ??= GetSessionForCurrentUser();

        protected abstract IAnalyticsSession GetSessionForCurrentUser();

    }
}