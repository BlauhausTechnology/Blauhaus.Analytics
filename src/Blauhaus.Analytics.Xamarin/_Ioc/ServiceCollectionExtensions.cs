using System.Diagnostics;
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
using Microsoft.Extensions.DependencyInjection;

namespace Blauhaus.Analytics.Xamarin._Ioc
{
    public static class ServiceCollectionExtensions
    {
        
        public static IServiceCollection RegisterXamarinAnalyticsService<TConfig>(this IServiceCollection services) 
            where TConfig : class, IApplicationInsightsConfig
        {
            services.RegisterConsoleLoggerClientService();
            
            services.AddScoped<IDeviceInfoService, DeviceInfoService>();
            services.AddScoped<IApplicationInfoService, ApplicationInfoService>();

            services.AddScoped<IApplicationInsightsConfig, TConfig>();
            services.AddScoped<ITelemetryClientProxy, TelemetryClientProxy>();
            services.AddScoped<ITelemetryDecorator, TelemetryDecorator>();

            services.AddScoped<IAnalyticsSessionFactory, XamarinSessionFactory>();
            services.AddScoped<IAnalyticsService, AnalyticsService>();
            return services;
        }


    }
}