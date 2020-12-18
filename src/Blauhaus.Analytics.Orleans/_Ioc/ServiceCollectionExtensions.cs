using System.Diagnostics;
using Blauhaus.Analytics.Abstractions.Config;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Analytics.Abstractions.Session;
using Blauhaus.Analytics.Common.Service;
using Blauhaus.Analytics.Common.Session;
using Blauhaus.Analytics.Common.Telemetry;
using Blauhaus.Analytics.Console._Ioc;
using Blauhaus.Analytics.Orleans.Context;
using Blauhaus.Analytics.Orleans.Session;
using Microsoft.Extensions.DependencyInjection;

namespace Blauhaus.Analytics.Orleans._Ioc
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
            
            services.AddScoped<IApplicationInsightsConfig, TConfig>();
            services.AddScoped<ITelemetryClientProxy, TelemetryClientProxy>();
            services.AddScoped<ITelemetryDecorator, TelemetryDecorator>();
            
            services.AddScoped<IAnalyticsSessionFactory, OrleansSessionFactory>();
            
            services.AddScoped<IAnalyticsService, OrleansAnalyticsService>();
            services.AddScoped<IOrleansRequestContext, OrleansRequestContext>();

            
            return services;
        }
    }
}