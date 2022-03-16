using Blauhaus.Analytics.Serilog.Ioc;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Blauhaus.Analytics.Serilog.AspNetCore.Ioc
{
    public static class ServiceCollectionExtensions 
    {
        public static IServiceCollection AddAspNetCoreSerilogAnalyticsService(this IServiceCollection services, Action<LoggerConfiguration> config)
        {

            services.AddSerilogAnalyticsService<SerilogAnalyticsService, AspNetCoreSessionFactory>(config);
            

            return services;
        }
    }
}