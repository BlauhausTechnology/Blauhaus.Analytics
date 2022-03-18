using Blauhaus.Analytics.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Blauhaus.Analytics.Serilog.Maui.Ioc;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMauiSerilogAnalyticsService(this IServiceCollection services, Action<LoggerConfiguration> config)
    {
        services.AddScoped<IAnalyticsContext, MauiAnalyticsContext>();
        services.AddTransient(typeof(IAnalyticsLogger<>), typeof(AnalyticsLogger<>));

        return services;
    }
}