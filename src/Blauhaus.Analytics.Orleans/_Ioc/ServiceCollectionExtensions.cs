using Blauhaus.Analytics.Abstractions.Config;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Analytics.Abstractions.Session;
using Blauhaus.Analytics.Common.Service;
using Blauhaus.Analytics.Common.Session;
using Blauhaus.Analytics.Common.Telemetry;
using Blauhaus.Analytics.Orleans.Context;
using Blauhaus.Analytics.Orleans.Session;
using Microsoft.Extensions.DependencyInjection;

namespace Blauhaus.Analytics.Orleans._Ioc
{
    public static class ServiceCollectionExtensions 
    {
        public static IServiceCollection AddOrleansAnalytics<TConfig>(this IServiceCollection services)
            where TConfig : class, IApplicationInsightsConfig
        {
            services.AddSingleton<IApplicationInsightsConfig, TConfig>();
            services.AddSingleton<ITelemetryClientProxy, TelemetryClientProxy>();
            services.AddSingleton<ITelemetryDecorator, TelemetryDecorator>();
            
            services.AddSingleton<IAnalyticsSessionFactory, OrleansSessionFactory>();
            
            services.AddSingleton<IAnalyticsService, OrleansAnalyticsService>();
            services.AddSingleton<IOrleansRequestContext, OrleansRequestContext>();

            
            return services;
        }
    }
}