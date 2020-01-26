using Blauhaus.AppInsights.Client.Service;
using Blauhaus.AppInsights.Server.Service;
using Blauhaus.AppInsights.Tests.Tests.AppInsightsServiceTests._BaseTests;

namespace Blauhaus.AppInsights.Tests.Tests.AppInsightsServiceTests.AppInsightsServerServiceTests
{
    public class ContinueOperationTests : BaseContinueOperationTests<AppInsightsServerService>
    {
        protected override AppInsightsServerService ConstructSut()
        {
            return new AppInsightsServerService(
                MockConfig.Object,
                MockConsoleLogger.Object,
                MockTelemetryClient.Object);
        }
    }
}