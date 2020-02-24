using Blauhaus.Analytics.Abstractions.Config;
using Blauhaus.Analytics.Abstractions.Session;
using Blauhaus.Analytics.Common.Service;
using Blauhaus.Analytics.Common.Telemetry;
using Blauhaus.Analytics.Console.ConsoleLoggers;
using Blauhaus.Common.ValueObjects.BuildConfigs;
using Blauhaus.DeviceServices.Abstractions.Application;
using Blauhaus.DeviceServices.Abstractions.DeviceInfo;

namespace Blauhaus.Analytics.Xamarin.Service
{
    public class XamarinAnalyticsService : AnalyticsService
    {
        public XamarinAnalyticsService(
            IApplicationInsightsConfig config,
            IConsoleLogger appInsightsLogger,
            ITelemetryClientProxy telemetryClient,
            ITelemetryDecorator telemetryDecorator,
            IBuildConfig currentBuildConfig,
            IDeviceInfoService deviceInfoService,
            IApplicationInfoService applicationInfoService)
            : base(config, appInsightsLogger, telemetryClient, telemetryDecorator, currentBuildConfig)
        {
            CurrentSession = AnalyticsSession.New;
            CurrentSession.AppVersion = applicationInfoService.CurrentVersion;
            CurrentSession.DeviceId = deviceInfoService.DeviceUniqueIdentifier;
        }



    }
}