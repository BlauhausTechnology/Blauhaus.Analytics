using Blauhaus.AppInsights.Abstractions.Config;
using Blauhaus.AppInsights.Abstractions.Service;
using Blauhaus.AppInsights.Server.Service;
using Blauhaus.Ioc.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Blauhaus.AppInsights.Server._Ioc
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterAppInsightsServer<TConfig>(this IServiceCollection services) 
            where TConfig : class, IApplicationInsightsConfig
        {
            services.AddScoped<IApplicationInsightsConfig, TConfig>();
            services.AddScoped<IAppInsightsService, AppInsightsServerService>();
            //var config = services.BuildServiceProvider().GetService<IApplicationInsightsConfig>();

            services.AddApplicationInsightsTelemetry("");

            return services;
        }
    }
}