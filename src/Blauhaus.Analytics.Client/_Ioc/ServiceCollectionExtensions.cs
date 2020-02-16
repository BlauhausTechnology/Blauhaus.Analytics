using System.Diagnostics;
using Blauhaus.Analytics.Abstractions.Config;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Analytics.Client.Service;
using Blauhaus.Analytics.Common.Telemetry;
using Blauhaus.Analytics.Console._Ioc;
using Blauhaus.DeviceServices.Abstractions.Application;
using Blauhaus.DeviceServices.Abstractions.DeviceInfo;
using Blauhaus.Ioc.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Blauhaus.Analytics.Client._Ioc
{
    public static class ServiceCollectionExtensions
    {
        
        public static IServiceCollection RegisterAnalyticsClientService<TConfig, TDeviceInfoService>(this IServiceCollection services, TraceListener consoleTraceListener) 
            where TConfig : class, IApplicationInsightsConfig 
            where TDeviceInfoService : class, IDeviceInfoService, IApplicationInfoService
        {
            RegisterCommon<TConfig, TDeviceInfoService>(services, consoleTraceListener);
            return services;
        }


        public static IServiceCollection RegisterAnalyticsClientService<TConfig>(this IServiceCollection services, TraceListener consoleTraceListener) 
            where TConfig : class, IApplicationInsightsConfig 
        {
            RegisterCommon<TConfig, DefaultDeviceService>(services, consoleTraceListener);
            return services;
        }

        private static IServiceCollection RegisterCommon<TConfig, TDeviceInfoService>(this IServiceCollection services, TraceListener consoleTraceListener) 
            where TConfig : class, IApplicationInsightsConfig            
            where TDeviceInfoService : class, IDeviceInfoService , IApplicationInfoService
        {
            services.RegisterConsoleLoggerService(consoleTraceListener);
            
            services.AddSingleton<IDeviceInfoService, TDeviceInfoService>();
            services.AddSingleton<IApplicationInfoService, TDeviceInfoService>();

            services.AddSingleton<IApplicationInsightsConfig, TConfig>();
            services.AddSingleton<ITelemetryClientProxy, TelemetryClientProxy>();
            services.AddSingleton<ITelemetryDecorator, TelemetryDecorator>();

            services.AddScoped<IAnalyticsClientService, AnalyticsClientService>();
            services.AddScoped<IAnalyticsService>(x => x.GetService<IAnalyticsClientService>());
            return services;
        }
    }
}