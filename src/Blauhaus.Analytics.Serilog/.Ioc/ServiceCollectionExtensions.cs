using System;
using Blauhaus.Analytics.Abstractions;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Analytics.Abstractions.Session;
using Blauhaus.Analytics.Console.ConsoleLoggers;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Exceptions;

namespace Blauhaus.Analytics.Serilog.Ioc
{
    public static class ServiceCollectionExtensions 
    {
        public static IServiceCollection AddSerilogAnalyticsService<TAnalyticsService, TSessionFactory>(this IServiceCollection services, string appName, Action<LoggerConfiguration> config) 
            where TSessionFactory : class, IAnalyticsSessionFactory
            where TAnalyticsService : class, IAnalyticsService
        {
            var loggerConfiguration = new LoggerConfiguration();
            config.Invoke(loggerConfiguration);
            Log.Logger = loggerConfiguration
                .CreateLogger();

            services.AddSerilogAnalytics(appName, config);
            services.AddScoped<IAnalyticsService, TAnalyticsService>();
            services.AddTransient<IAnalyticsSessionFactory, TSessionFactory>();
            services.AddTransient<IConsoleLogger, ConsoleLogger>();
            return services;
        }

        public static IServiceCollection AddSerilogAnalytics(this IServiceCollection services, string appName, Action<LoggerConfiguration> config)
        {
            var configuration = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithExceptionDetails()
                .Destructure.ToMaximumDepth(5)
                .Enrich.WithProperty("AppName", appName);
            config.Invoke(configuration);
            Log.Logger = configuration.CreateLogger();

            services.AddTransient(typeof(IAnalyticsLogger<>), typeof(AnalyticsLogger<>));
            return services;
        }
    }
}