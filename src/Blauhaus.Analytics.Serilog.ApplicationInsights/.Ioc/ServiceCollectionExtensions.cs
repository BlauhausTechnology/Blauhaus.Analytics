using System;
using Blauhaus.Analytics.Serilog.Ioc;
using Microsoft.ApplicationInsights.Extensibility.Implementation;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Blauhaus.Analytics.Serilog.ApplicationInsights.Ioc;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSerilogApplicationInsights(this IServiceCollection services,  string appName, Action<LoggerConfiguration> config)
    {
        TelemetryDebugWriter.IsTracingDisabled = true;
        services.AddSerilogAnalytics(appName, config);       
        
        return services;
    }
}