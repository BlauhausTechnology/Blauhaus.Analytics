using System;
using Blauhaus.AppInsights.Abstractions.ConsoleLoggers;
using Blauhaus.Ioc.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Blauhaus.AppInsights.Abstractions._Ioc
{
    public static class IocServiceExtensions
    {
        public static IIocService RegisterConsoleLogger(this IIocService services)
        {
            services.RegisterImplementation<ITraceProxy, TraceProxy>();
            services.RegisterImplementation<IConsoleLogger, ConsoleLogger>();

            return services;
        }
    }
}
