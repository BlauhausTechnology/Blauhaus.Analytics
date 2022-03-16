using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Analytics.Abstractions.Session;
using Blauhaus.Analytics.Common.Session;
using Microsoft.Extensions.DependencyInjection;

namespace Blauhaus.Analytics.ILogger.Ioc;

public static class ServiceCollectionSestions
{
    public static IServiceCollection AddLoggerAnalyticsService(this IServiceCollection services)
    {
        services.AddSingleton<IAnalyticsService, LoggerAnalyticsService>();
        services.AddSingleton<IAnalyticsSessionFactory, AnalyticsSessionFactory>();

        return services;
    }

     
}