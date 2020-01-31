using System.Diagnostics;
using Blauhaus.Analytics.Abstractions.Config;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Analytics.Common.Telemetry;
using Blauhaus.Analytics.Console._Ioc;
using Blauhaus.Analytics.Server.Service;
using Microsoft.ApplicationInsights;
using Microsoft.Extensions.DependencyInjection;

namespace Blauhaus.Analytics.Server._Ioc
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterAnalyticsServerService<TConfig>(this IServiceCollection services, TraceListener consoleTraceListener) 
            where TConfig : class, IApplicationInsightsConfig
        {
            services.RegisterConsoleLoggerServerService(consoleTraceListener);

            services.AddScoped<IApplicationInsightsConfig, TConfig>();
            services.AddScoped<IAnalyticsServerService, AnalyticsServerService>();
            services.AddScoped<ITelemetryClientProxy, TelemetryClientProxy>();
            services.AddScoped<ITelemetryDecorator, TelemetryDecorator>();
            services.AddScoped<IAnalyticsService>(x => x.GetService<IAnalyticsServerService>());

            services.AddScoped<TelemetryClient>();

            return services;
        }
    }
}