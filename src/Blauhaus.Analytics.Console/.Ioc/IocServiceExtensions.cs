using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Analytics.Console.ConsoleLoggers;
using Blauhaus.Analytics.Console.Service;
using Blauhaus.Ioc.Abstractions;

namespace Blauhaus.Analytics.Console.Ioc
{
    public static class IocServiceExtensions
    {

        public static IIocService RegisterConsoleLoggerClientService(this IIocService iocService)
        {
            iocService.RegisterImplementation<ITraceProxy, TraceProxy>(IocLifetime.Singleton);
            iocService.RegisterImplementation<IConsoleLogger, ConsoleLogger>(IocLifetime.Singleton);
            iocService.RegisterImplementation<IAnalyticsService, ConsoleLoggerService>(IocLifetime.Singleton);

            return iocService;
        }
    }
}
