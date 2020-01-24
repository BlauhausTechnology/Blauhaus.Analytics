using Blauhaus.AppInsights.Abstractions.Service;
using Blauhaus.AppInsights.Server._Ioc;
using Blauhaus.AppInsights.Test.Tests._Base;
using Blauhaus.AppInsights.Test.Tests.Config;
using Microsoft.Extensions.DependencyInjection;

namespace Blauhaus.AppInsights.Test.Tests
{
    public class ServerTests : BaseTest
    {
        protected override IAppInsightsService GetAppInsightsService()
        {
            var services = new ServiceCollection();

            services.RegisterAppInsightsServer<TestAppInsightsConfig>();

            return services.BuildServiceProvider().GetService<IAppInsightsService>();
        }
    }
}