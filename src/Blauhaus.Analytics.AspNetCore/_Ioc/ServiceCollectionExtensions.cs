using System.Diagnostics;
using Blauhaus.Analytics.Abstractions.Config;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Analytics.Common.Service;
using Blauhaus.Analytics.Common.Telemetry;
using Blauhaus.Analytics.Console._Ioc;
using Microsoft.ApplicationInsights;
using Microsoft.Extensions.DependencyInjection;

namespace Blauhaus.Analytics.AspNetCore._Ioc
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterAspNetCoreAnalyticsService<TConfig>(this IServiceCollection services, TraceListener consoleTraceListener) 
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