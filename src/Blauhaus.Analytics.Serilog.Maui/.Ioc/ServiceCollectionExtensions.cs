using Blauhaus.Analytics.Abstractions;
using Blauhaus.Analytics.Serilog.Ioc;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Blauhaus.Analytics.Serilog.Maui.Ioc;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMauiSerilogAnalytics(this IServiceCollection services, string appName, Action<LoggerConfiguration> config)
    {
        services.AddSerilogAnalytics<MauiAnalyticsContext>(appName, config); 
        return services;
    }
}