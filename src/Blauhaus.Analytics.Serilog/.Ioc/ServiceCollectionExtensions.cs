using System;
using Blauhaus.Analytics.Abstractions;
using Blauhaus.Analytics.Abstractions.Config;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Analytics.Abstractions.Session;
using Blauhaus.Analytics.Common.Telemetry;
using Blauhaus.Analytics.Console.ConsoleLoggers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Serilog;
using Serilog.Exceptions;

namespace Blauhaus.Analytics.Serilog.Ioc
{
    public static class ServiceCollectionExtensions 
    {
        public static IServiceCollection AddSerilogAnalyticsService<TAnalyticsService, TSessionFactory, TContext>(this IServiceCollection services, string appName, Action<LoggerConfiguration> config) 
            where TSessionFactory : class, IAnalyticsSessionFactory
            where TAnalyticsService : class, IAnalyticsService
            where TContext : class, IAnalyticsContext
        { 
            services.AddSerilogAnalytics(appName, config);

            services.AddScoped<IAnalyticsContext, TContext>();
            services.AddScoped<IAnalyticsService, TAnalyticsService>();
            services.AddTransient<IAnalyticsSessionFactory, TSessionFactory>();
            
            return services;
        }
        
        public static IServiceCollection AddSerilogAnalytics<TContext>(this IServiceCollection services, string appName, Action<LoggerConfiguration> config) 
            where TContext : class, IAnalyticsContext
        { 
            var configuration = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithExceptionDetails()
                .Destructure.ToMaximumDepth(5)
                .Enrich.WithProperty("AppName", appName);
            config.Invoke(configuration);

            Log.Logger = configuration.CreateLogger();
            
            services.AddTransient(typeof(IAnalyticsLogger<>), typeof(AnalyticsLogger<>));
            services.TryAddSingleton<IAnalyticsContext, TContext>(); 
            services.AddLogging(logging => { logging.AddSerilog(dispose:true); });
            
            return services;
        }

        public static IServiceCollection AddSerilogAnalytics(this IServiceCollection services, string appName, Action<LoggerConfiguration> config)
        {
            return services.AddSerilogAnalytics<InMemoryAnalyticsContext>(appName, config);
        }
    }
}