using System.Diagnostics;
using Blauhaus.Analytics.Abstractions;
using Blauhaus.Analytics.Abstractions.Config;
using Blauhaus.Analytics.Abstractions.Session;
using Blauhaus.Analytics.Console.Ioc;
using Blauhaus.Analytics.Orleans;
using Blauhaus.Analytics.Orleans.Ioc;
using Blauhaus.Analytics.Orleans.Session;
using Blauhaus.Analytics.Serilog.Ioc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Orleans;
using Serilog;
using Serilog.Events;

namespace Blauhaus.Analytics.Serilog.Orleans.Ioc
{
    public static class ServiceCollectionExtensions 
    {
        public static IServiceCollection AddOrleansSerilogAnalyticsService(this IServiceCollection services, string appName, IConfiguration? configuration, Action<LoggerConfiguration> config)
        {
            services.AddSerilogAnalyticsService<OrleansAnalyticsService, OrleansSessionFactory, OrleansAnalyticsContext>(appName, configuration, config);
            services.AddOrleansAnalytics<DefaultApplicationInsightsConfig>(new ConsoleTraceListener());

            return services;
            
        }
        
        public static IServiceCollection AddOrleansSerilogAnalytics(this IServiceCollection services, string appName, IConfiguration? configuration, Action<LoggerConfiguration> config)
        {
            services.AddSerilogAnalytics(appName, configuration, config);
            services.AddTransient<IAnalyticsContext, OrleansAnalyticsContext>();
            services.AddSingleton<IIncomingGrainCallFilter, AnalyticsGrainFilter>();
            
            return services;
        }
    }
}