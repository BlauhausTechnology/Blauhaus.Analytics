using Blauhaus.Analytics.Abstractions.Config;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Analytics.Abstractions.Session;
using Blauhaus.Analytics.Common.Service;
using Blauhaus.Analytics.Common.Telemetry;
using Blauhaus.Analytics.Console.Ioc;
using Blauhaus.Analytics.Xamarin.SessionFactories;
using Microsoft.Extensions.DependencyInjection;

namespace Blauhaus.Analytics.Xamarin.AppCenter.Ioc
{
    public static class ServiceCollectionExtensions
    {
        
        public static IServiceCollection AddXamarinAppCenterAnalyticsService<TConfig>(this IServiceCollection services) 
            where TConfig : class, IApplicationInsightsConfig
        {
            services.RegisterConsoleLoggerClientService();

            services.AddSingleton<IApplicationInsightsConfig, TConfig>();
            services.AddSingleton<ITelemetryClientProxy, TelemetryClientProxy>();
            services.AddSingleton<ITelemetryDecorator, TelemetryDecorator>();

            services.AddSingleton<IAnalyticsSessionFactory, XamarinSessionFactory>();
            services.AddSingleton<IAnalyticsService, AppCenterAnalyticsService>();
            return services;
        }


    }
}