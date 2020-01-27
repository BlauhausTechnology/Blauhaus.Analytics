﻿using System.Diagnostics;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Analytics.Console.ConsoleLoggers;
using Blauhaus.Analytics.Console.Service;
using Microsoft.Extensions.DependencyInjection;

namespace Blauhaus.Analytics.Console._Ioc
{
    public static class ServiceCollectionExtensions
    {

        public static IServiceCollection RegisterConsoleLogger(this IServiceCollection services, TraceListener consoleTraceListener)
        {
            
            //so that console logging works
            Trace.Listeners.Add(consoleTraceListener);

            services.AddScoped<ITraceProxy, TraceProxy>();
            services.AddScoped<IConsoleLogger, ConsoleLogger>();

            return services;
        }


        public static IServiceCollection RegisterConsoleLoggerServerService(this IServiceCollection services, TraceListener consoleTraceListener)
        {
            services.RegisterConsoleLogger(consoleTraceListener);
            services.AddScoped<IAnalyticsServerService, ConsoleLoggerService>();
            services.AddScoped<IAnalyticsService>(x => x.GetService<IAnalyticsServerService>());

            return services;
        }
    }
}
