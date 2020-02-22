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
        
        public static IServiceCollection RegisterAnalyticsClientService<TConfig, TDeviceInfoService, TApplicationInfoService>(this IServiceCollection services, TraceListener consoleTraceListener) 
            where TConfig : class, IApplicationInsightsConfig 
            where TDeviceInfoService : class, IDeviceInfoService 
            where TApplicationInfoService : class, IApplicationInfoService
        {
            RegisterCommon<TConfig, TDeviceInfoService, TApplicationInfoService>(services, consoleTraceListener);
            return services;
        }


        public static IServiceCollection RegisterAnalyticsClientService<TConfig>(this IServiceCollection services, TraceListener consoleTraceListener) 
            where TConfig : class, IApplicationInsightsConfig 
        {
            RegisterCommon<TConfig, DefaultDeviceService, DefaultDeviceService>(services, consoleTraceListener);
            return services;
        }

        private static IServiceCollection RegisterCommon<TConfig, TDeviceInfoService, TApplicationInfoService>(this IServiceCollection services, TraceListener consoleTraceListener) 
            where TConfig : class, IApplicationInsightsConfig            
            where TDeviceInfoService : class, IDeviceInfoService 
            where TApplicationInfoService : class, IApplicationInfoService
        {
            services.RegisterConsoleLoggerService(consoleTraceListener);
            
            services.AddScoped<IDeviceInfoService, TDeviceInfoService>();
            services.AddScoped<IApplicationInfoService, TApplicationInfoService>();

            services.AddScoped<IApplicationInsightsConfig, TConfig>();
            services.AddScoped<ITelemetryClientProxy, TelemetryClientProxy>();
            services.AddScoped<ITelemetryDecorator, TelemetryDecorator>();

            services.AddScoped<IAnalyticsClientService, AnalyticsClientService>();
            services.AddScoped<IAnalyticsService>(x => x.GetService<IAnalyticsClientService>());
            return services;
        }
    }
}