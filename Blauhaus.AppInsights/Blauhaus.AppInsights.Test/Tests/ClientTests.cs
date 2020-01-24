using Blauhaus.AppInsights.Abstractions.Service;
using Blauhaus.AppInsights.Client._Ioc;
using Blauhaus.AppInsights.Test.Tests._Base;
using Blauhaus.AppInsights.Test.Tests.Config;
using Blauhaus.Ioc.DotNetCoreIocService;
using Microsoft.Extensions.DependencyInjection;

namespace Blauhaus.AppInsights.Test.Tests
{
    public class ClientTests : BaseTest
    {
        protected override IAppInsightsService GetAppInsightsService()
        {
            var services = new ServiceCollection();
            var iocService = new DotNetCoreIocService(services);
            iocService.RegisterAppInsightsClient<TestAppInsightsConfig>();
            return iocService.Resolve<IAppInsightsService>();
        }
    }
}