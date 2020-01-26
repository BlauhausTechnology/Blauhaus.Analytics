using Blauhaus.AppInsights.Abstractions._Ioc;
using Blauhaus.AppInsights.Abstractions.Config;
using Blauhaus.AppInsights.Abstractions.Service;
using Blauhaus.AppInsights.Client.Service;
using Blauhaus.Ioc.Abstractions;

namespace Blauhaus.AppInsights.Client._Ioc
{
    public static class IocServiceExtensions
    {
        public static IIocService RegisterAppInsightsClient<TConfig>(this IIocService iocService) 
            where TConfig : class, IApplicationInsightsConfig
        {
            
            iocService.RegisterConsoleLogger();

            iocService.RegisterImplementation<IApplicationInsightsConfig, TConfig>(IocLifetime.Singleton);
            iocService.RegisterImplementation<IAppInsightsClientService, AppInsightsClientService>(IocLifetime.Singleton);
            iocService.RegisterInstance<IAppInsightsService>(iocService.Resolve<IAppInsightsClientService>());


            return iocService;
        }
    }
}