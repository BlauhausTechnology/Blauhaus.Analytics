using Blauhaus.Analytics.Abstractions.Config;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Analytics.Abstractions.TelemetryClients;
using Blauhaus.Analytics.Client.Service;
using Blauhaus.Analytics.Console._Ioc;
using Blauhaus.AppInsights.Abstractions.TelemetryClients;
using Blauhaus.Ioc.Abstractions;

namespace Blauhaus.Analytics.Client._Ioc
{
    public static class IocServiceExtensions
    {
        public static IIocService RegisterAppInsightsClient<TConfig>(this IIocService iocService) 
            where TConfig : class, IApplicationInsightsConfig
        {
            iocService.RegisterConsoleLoggerClientService();

            iocService.RegisterImplementation<IApplicationInsightsConfig, TConfig>(IocLifetime.Singleton);
            iocService.RegisterImplementation<IAnalyticsClientService, AnalyticsClientService>(IocLifetime.Singleton);
            iocService.RegisterImplementation<ITelemetryClientProxy, TelemetryClientProxy>(IocLifetime.Singleton);
            iocService.RegisterInstance<IAnalyticsService>(iocService.Resolve<IAnalyticsClientService>());

            return iocService;
        }
    }
}