using System.Diagnostics;
using Blauhaus.Analytics.Abstractions.Config;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Analytics.Abstractions.TelemetryClients;
using Blauhaus.Analytics.Console._Ioc;
using Blauhaus.Analytics.Server.Service;
using Blauhaus.AppInsights.Abstractions.TelemetryClients;
using Microsoft.Extensions.DependencyInjection;

namespace Blauhaus.Analytics.Server._Ioc
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterAppInsightsServer<TConfig>(this IServiceCollection services, TraceListener consoleTraceListener) 
            where TConfig : class, IApplicationInsightsConfig
        {
            services.RegisterConsoleLogger(consoleTraceListener);

            services.AddScoped<IApplicationInsightsConfig, TConfig>();
            services.AddScoped<IAppInsightsServerService, AppInsightsServerService>();
            services.AddScoped<ITelemetryClientProxy, TelemetryClientProxy>();
            services.AddScoped<IAppInsightsService>(x => x.GetService<IAppInsightsServerService>());

            
            services.AddApplicationInsightsTelemetry();

            return services;
        }
    }
}