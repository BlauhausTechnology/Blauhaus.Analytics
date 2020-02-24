using Blauhaus.Analytics.Abstractions.Config;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Analytics.Common.Telemetry;
using Blauhaus.Analytics.Console._Ioc;
using Blauhaus.Analytics.Xamarin.Service;
using Blauhaus.DeviceServices.Abstractions.Application;
using Blauhaus.DeviceServices.Abstractions.DeviceInfo;
using Blauhaus.Ioc.Abstractions;

namespace Blauhaus.Analytics.Xamarin._Ioc
{
    public static class IocServiceExtensions
    {
        
        public static IIocService RegisterXamarinAnalyticsService<TConfig, TDeviceInfoService, TApplicationInfoService>(this IIocService iocService) 
            where TConfig : class, IApplicationInsightsConfig 
            where TDeviceInfoService : class, IDeviceInfoService 
            where TApplicationInfoService : class, IApplicationInfoService
        {
            iocService.RegisterConsoleLoggerClientService();
            
            iocService.RegisterImplementation<IDeviceInfoService, TDeviceInfoService>(IocLifetime.Singleton);
            iocService.RegisterImplementation<IApplicationInfoService, TApplicationInfoService>(IocLifetime.Singleton);

            iocService.RegisterImplementation<IApplicationInsightsConfig, TConfig>(IocLifetime.Singleton);
            iocService.RegisterImplementation<ITelemetryClientProxy, TelemetryClientProxy>(IocLifetime.Singleton);
            iocService.RegisterImplementation<ITelemetryDecorator, TelemetryDecorator>();

            iocService.RegisterImplementation<IAnalyticsService, XamarinAnalyticsService>(IocLifetime.Singleton);

            return iocService;
        }

    }
}