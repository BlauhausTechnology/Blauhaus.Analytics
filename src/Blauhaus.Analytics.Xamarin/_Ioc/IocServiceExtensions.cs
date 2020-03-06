using Blauhaus.Analytics.Abstractions.Config;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Analytics.Abstractions.Session;
using Blauhaus.Analytics.Common.Service;
using Blauhaus.Analytics.Common.Telemetry;
using Blauhaus.Analytics.Console._Ioc;
using Blauhaus.Analytics.Xamarin.SessionFactories;
using Blauhaus.DeviceServices.Abstractions.Application;
using Blauhaus.DeviceServices.Abstractions.DeviceInfo;
using Blauhaus.DeviceServices.Application;
using Blauhaus.DeviceServices.DeviceInfo;
using Blauhaus.Ioc.Abstractions;

namespace Blauhaus.Analytics.Xamarin._Ioc
{
    public static class IocServiceExtensions
    {
        
        public static IIocService RegisterXamarinAnalyticsService<TConfig>(this IIocService iocService) 
            where TConfig : class, IApplicationInsightsConfig 
        {
            iocService.RegisterConsoleLoggerClientService();
            
            iocService.RegisterImplementation<IDeviceInfoService, DeviceInfoService>(IocLifetime.Singleton);
            iocService.RegisterImplementation<IApplicationInfoService, ApplicationInfoService>(IocLifetime.Singleton);

            iocService.RegisterImplementation<IApplicationInsightsConfig, TConfig>(IocLifetime.Singleton);
            iocService.RegisterImplementation<ITelemetryClientProxy, TelemetryClientProxy>(IocLifetime.Singleton);
            iocService.RegisterImplementation<ITelemetryDecorator, TelemetryDecorator>();

            iocService.RegisterImplementation<IAnalyticsSessionFactory, XamarinSessionFactory>(IocLifetime.Singleton);
            iocService.RegisterImplementation<IAnalyticsService, AnalyticsService>(IocLifetime.Singleton);

            return iocService;
        }

    }
}