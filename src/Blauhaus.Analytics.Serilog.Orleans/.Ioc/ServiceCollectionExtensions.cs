using System.Diagnostics;
using Blauhaus.Analytics.Abstractions;
using Blauhaus.Analytics.Abstractions.Config;
using Blauhaus.Analytics.Console.Ioc;
using Blauhaus.Analytics.Orleans.Ioc;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;

namespace Blauhaus.Analytics.Serilog.Orleans.Ioc
{
    public static class ServiceCollectionExtensions 
    {
        public static IServiceCollection AddOrleansSerilogAnalyticsService(this IServiceCollection services, Action<LoggerConfiguration> config)
        {
            var configuration = new LoggerConfiguration();
            config.Invoke(configuration);
            Log.Logger = configuration.CreateLogger();

            services.AddScoped<IAnalyticsContext, OrleansAnalyticsContext>();
            services.AddTransient(typeof(IAnalyticsLogger<>), typeof(AnalyticsLogger<>));

            services.AddOrleansAnalytics<DefaultApplicationInsightsConfig>(new ConsoleTraceListener());

            return services;
        }
    }
}