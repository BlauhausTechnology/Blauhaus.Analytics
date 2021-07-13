using System.Diagnostics;
using Blauhaus.Analytics.Abstractions.Config;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Analytics.Abstractions.Session;
using Blauhaus.Analytics.AspNetCore.SessionFactories;
using Blauhaus.Analytics.Common.Service;
using Blauhaus.Analytics.Common.Telemetry;
using Blauhaus.Analytics.Console.Ioc;
using Microsoft.ApplicationInsights;
using Microsoft.Extensions.DependencyInjection;

namespace Blauhaus.Analytics.AspNetCore.Ioc
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterAspNetCoreWebAnalyticsService<TConfig>(this IServiceCollection services, TraceListener consoleTraceListener) 
            where TConfig : class, IApplicationInsightsConfig
        {

            services.RegisterCommon<TConfig>(consoleTraceListener);

            services.AddHttpContextAccessor();
            services.AddScoped<IAnalyticsSessionFactory, AspNetCoreWebSessionFactory>();
            return services;
        }

        public static IServiceCollection RegisterAspNetCorApiAnalyticsService<TConfig>(this IServiceCollection services, TraceListener consoleTraceListener) 
            where TConfig : class, IApplicationInsightsConfig
        {
            services.RegisterCommon<TConfig>(consoleTraceListener);
            services.AddScoped<IAnalyticsSessionFactory, AspNetCoreApiSessionFactory>();
            return services;
        }

        private static IServiceCollection RegisterCommon<TConfig>(this IServiceCollection services, TraceListener consoleTraceListener) 
            where TConfig : class, IApplicationInsightsConfig
        {
            services.RegisterConsoleLoggerService(consoleTraceListener);

            services.AddScoped<IApplicationInsightsConfig, TConfig>();
            services.AddScoped<ITelemetryClientProxy, TelemetryClientProxy>();
            services.AddScoped<ITelemetryDecorator, TelemetryDecorator>();
            services.AddScoped<TelemetryClient>();
            services.AddScoped<IAnalyticsService, AnalyticsService>();

            return services;
        }
    }
}