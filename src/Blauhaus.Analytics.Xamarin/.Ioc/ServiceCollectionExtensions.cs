using Blauhaus.Analytics.Abstractions;
using Blauhaus.Analytics.Abstractions.Config;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Analytics.Abstractions.Session;
using Blauhaus.Analytics.Common.Service;
using Blauhaus.Analytics.Common.Telemetry;
using Blauhaus.Analytics.Console.Ioc;
using Blauhaus.Analytics.Serilog;
using Blauhaus.Analytics.Xamarin.SessionFactories;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Diagnostics;
using Blauhaus.Analytics.Serilog.Ioc;
using Microsoft.Extensions.Configuration;

namespace Blauhaus.Analytics.Xamarin.Ioc
{
    public static class ServiceCollectionExtensions
    {
        
        public static IServiceCollection AddXamarinAnalyticsService<TConfig>(this IServiceCollection services) 
            where TConfig : class, IApplicationInsightsConfig
        {
            services.RegisterConsoleLoggerClientService();

            services.AddSingleton<IApplicationInsightsConfig, TConfig>();
            services.AddSingleton<ITelemetryClientProxy, TelemetryClientProxy>();
            services.AddSingleton<ITelemetryDecorator, TelemetryDecorator>();

            services.AddSingleton<IAnalyticsSessionFactory, XamarinSessionFactory>();
            services.AddSingleton<IAnalyticsService, AnalyticsService>();

            services.AddSingleton<IAnalyticsContext, XamarinAnalyticsContext>();

            services.AddTransient(typeof(IAnalyticsLogger<>), typeof(DummyAnalyticsLogger<>));
            return services;
        }

        public static IServiceCollection AddXamarinSerilogAnalytics(this IServiceCollection services, string appName, Action<LoggerConfiguration> configureLogger)
        {
            return services.AddSerilogAnalytics<XamarinAnalyticsContext>(appName, configureLogger);
        }

    }
}