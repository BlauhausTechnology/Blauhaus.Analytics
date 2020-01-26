using System.Diagnostics;
using Blauhaus.AppInsights.Abstractions._Ioc;
using Blauhaus.AppInsights.Abstractions.Config;
using Blauhaus.AppInsights.Abstractions.Service;
using Blauhaus.AppInsights.Server.Service;
using Blauhaus.Ioc.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;

namespace Blauhaus.AppInsights.Server._Ioc
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterAppInsightsServer<TConfig>(this IServiceCollection services, TraceListener consoleTraceListener) 
            where TConfig : class, IApplicationInsightsConfig
        {
            
            //so that console logging works
            Trace.Listeners.Add(consoleTraceListener);

            services.AddScoped<IApplicationInsightsConfig, TConfig>();
            services.AddScoped<IAppInsightsServerService, AppInsightsServerService>();
            services.AddScoped<IAppInsightsService>(x => x.GetService<IAppInsightsServerService>());

            services.RegisterConsoleLogger();
            
            services.AddApplicationInsightsTelemetry();

            return services;
        }
    }
}