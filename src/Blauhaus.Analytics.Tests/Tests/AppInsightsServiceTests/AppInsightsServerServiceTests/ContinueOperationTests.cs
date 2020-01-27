using Blauhaus.Analytics.Server.Service;
using Blauhaus.Analytics.Tests.Tests.AppInsightsServiceTests._BaseTests;

namespace Blauhaus.Analytics.Tests.Tests.AppInsightsServiceTests.AppInsightsServerServiceTests
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