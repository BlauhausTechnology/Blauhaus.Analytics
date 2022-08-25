﻿using System;
using Blauhaus.Analytics.Abstractions;
using Blauhaus.Analytics.Abstractions.Config;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Analytics.Abstractions.Session;
using Blauhaus.Analytics.Common.Telemetry;
using Blauhaus.Analytics.Console.ConsoleLoggers;
using Blauhaus.Analytics.Serilog.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Serilog;
using Serilog.Exceptions;

namespace Blauhaus.Analytics.Serilog.Ioc
{
    public static class ServiceCollectionExtensions 
    {
        public static IServiceCollection AddSerilogAnalyticsService<TAnalyticsService, TSessionFactory, TContext>(this IServiceCollection services, string appName, IConfiguration? configuration, Action<LoggerConfiguration> config) 
            where TSessionFactory : class, IAnalyticsSessionFactory
            where TAnalyticsService : class, IAnalyticsService
            where TContext : class, IAnalyticsContext
        { 
            services.AddSerilogAnalytics(appName, configuration, config);

            services.AddScoped<IAnalyticsContext, TContext>();
            services.AddScoped<IAnalyticsService, TAnalyticsService>();
            services.AddTransient<IAnalyticsSessionFactory, TSessionFactory>();
            
            return services;
        }
        
        public static IServiceCollection AddSerilogAnalytics<TContext>(this IServiceCollection services, string appName, IConfiguration? configuration, Action<LoggerConfiguration> config) 
            where TContext : class, IAnalyticsContext
        { 
            var logging = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithExceptionDetails()
                .Destructure.ToMaximumDepth(5)
                .Enrich.WithProperty("AppName", appName)
                .WriteTo.Debug();
            config.Invoke(logging);

            if (configuration is not null)
            {
                logging.ApplyLoggingConfiguration(configuration);
            }
            
            Log.Logger = logging.CreateLogger();
            
            services.AddTransient(typeof(IAnalyticsLogger<>), typeof(AnalyticsLogger<>));
            services.TryAddSingleton<IAnalyticsContext, TContext>(); 
            services.AddLogging(logging => { logging.AddSerilog(dispose:true); });
            
            return services;
        }

        public static IServiceCollection AddSerilogAnalytics(this IServiceCollection services, string appName, IConfiguration? configuration, Action<LoggerConfiguration> config)
        {
            return services.AddSerilogAnalytics<InMemoryAnalyticsContext>(appName, configuration, config);
        }
    }
}