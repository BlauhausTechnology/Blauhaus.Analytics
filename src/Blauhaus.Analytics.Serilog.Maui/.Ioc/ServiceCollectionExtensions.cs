using Blauhaus.Analytics.Abstractions;
using Blauhaus.Analytics.Serilog.Ioc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Blauhaus.Analytics.Serilog.Maui.Ioc;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMauiSerilogAnalytics(this IServiceCollection services, string appName, IConfiguration? configuration, Action<LoggerConfiguration> logging)
    {
        services.AddSerilogAnalytics<MauiAnalyticsContext>(appName, configuration, logging); 
        return services;
    }
}