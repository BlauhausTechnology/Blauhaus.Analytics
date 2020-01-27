using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Analytics.Console.ConsoleLoggers;
using Blauhaus.Analytics.Console.Service;
using Blauhaus.Ioc.Abstractions;

namespace Blauhaus.Analytics.Console._Ioc
{
    public static class IocServiceExtensions
    {



        public static IIocService RegisterConsoleLogger(this IIocService iocService)
        {
            iocService.RegisterImplementation<ITraceProxy, TraceProxy>();
            iocService.RegisterImplementation<IConsoleLogger, ConsoleLogger>();
            return iocService;
        }

        public static IIocService RegisterConsoleLoggerClientService(this IIocService iocService)
        {
            iocService.RegisterConsoleLogger();
            iocService.RegisterImplementation<IAppInsightsClientService, ConsoleLoggerService>(IocLifetime.Singleton);
            iocService.RegisterInstance<IAppInsightsService>(iocService.Resolve<IAppInsightsClientService>());

            return iocService;
        }
    }
}
