using System.Diagnostics;
using Blauhaus.Analytics.Abstractions;
using Blauhaus.Analytics.Abstractions.Config;
using Blauhaus.Analytics.Console.Ioc;
using Blauhaus.Analytics.Orleans;
using Blauhaus.Analytics.Orleans.Ioc;
using Blauhaus.Analytics.Orleans.Session;
using Blauhaus.Analytics.Serilog.Ioc;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;

namespace Blauhaus.Analytics.Serilog.Orleans.Ioc
{
    public static class ServiceCollectionExtensions 
    {
        public static IServiceCollection AddOrleansSerilogAnalyticsService(this IServiceCollection services, string appName, Action<LoggerConfiguration> config)
        {
            services.AddSerilogAnalyticsService<OrleansAnalyticsService, OrleansSessionFactory>(appName, config);
            services.AddScoped<IAnalyticsContext, OrleansAnalyticsContext>();

            //services.AddOrleansAnalytics<DefaultApplicationInsightsConfig>(new ConsoleTraceListener());

            return services;
        }
    }
}