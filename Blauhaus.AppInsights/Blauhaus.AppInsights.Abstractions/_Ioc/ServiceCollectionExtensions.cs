using System;
using Blauhaus.AppInsights.Abstractions.ConsoleLoggers;
using Microsoft.Extensions.DependencyInjection;

namespace Blauhaus.AppInsights.Abstractions._Ioc
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterConsoleLogger(this IServiceCollection services)
        {
            services.AddTransient<ITraceProxy, TraceProxy>();
            services.AddTransient<IConsoleLogger, ConsoleLogger>();

            return services;
        }
    }
}
