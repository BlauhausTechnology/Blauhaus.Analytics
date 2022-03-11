using System.Diagnostics;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Analytics.Console.ConsoleLoggers;
using Blauhaus.Analytics.Console.Service;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Blauhaus.Analytics.Console.Ioc
{
    public static class ServiceCollectionExtensions
    {
        private static bool _isTraceListenerRegistered;
        public static IServiceCollection RegisterConsoleLoggerClientService(this IServiceCollection services)
        {
            services.AddScoped<ITraceProxy, TraceProxy>();
            services.AddScoped<IConsoleLogger, ConsoleLogger>();
            services.AddScoped<IAnalyticsService, ConsoleLoggerService>();
            return services;
        }

        public static IServiceCollection RegisterConsoleLoggerService(this IServiceCollection services, TraceListener consoleTraceListener)
        {
            AddTraceListener(consoleTraceListener);
            services.TryAddScoped<ITraceProxy, TraceProxy>();
            services.TryAddScoped<IConsoleLogger, ConsoleLogger>();
            services.TryAddTransient<IAnalyticsService, ConsoleLoggerService>();

            return services;
        }

        private static void AddTraceListener(TraceListener consoleTraceListener)
        {
            if (!_isTraceListenerRegistered && Trace.Listeners.Count == 0)
            {
                //so that console logging works
                Trace.Listeners.Add(consoleTraceListener);
                _isTraceListenerRegistered = true;
            }
        }
    }
}
