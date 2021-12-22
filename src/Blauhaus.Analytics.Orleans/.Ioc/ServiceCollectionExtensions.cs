using System.Diagnostics;
using Blauhaus.Analytics.Abstractions.Config;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Analytics.Abstractions.Session;
using Blauhaus.Analytics.Common.Telemetry;
using Blauhaus.Analytics.Console.Ioc;
using Blauhaus.Analytics.Orleans.Context;
using Blauhaus.Analytics.Orleans.Session;
using Microsoft.Extensions.DependencyInjection;

namespace Blauhaus.Analytics.Orleans.Ioc
{
    public static class ServiceCollectionExtensions
    {
        private static TraceListener? _traceListener;
        
        public static IServiceCollection AddOrleansAnalytics<TConfig>(this IServiceCollection services, TraceListener consoleTraceListener)
            where TConfig : class, IApplicationInsightsConfig
        {
            if (_traceListener == null)
            {
                _traceListener = consoleTraceListener;
                services.RegisterConsoleLoggerService(_traceListener);
            }
            
            services.AddSingleton<IApplicationInsightsConfig, TConfig>();
            services.AddSingleton<ITelemetryClientProxy, TelemetryClientProxy>();
            services.AddSingleton<ITelemetryDecorator, TelemetryDecorator>();
            
            services.AddTransient<IAnalyticsSessionFactory, OrleansSessionFactory>();
            services.AddTransient<IAnalyticsService, OrleansAnalyticsService>();
            services.AddTransient<IOrleansRequestContext, OrleansRequestContext>();

            
            return services;
        }
    }
}