using Blauhaus.Analytics.Server.Service;
using Blauhaus.Analytics.Tests.Tests.AppInsightsServiceTests._BaseTests;

namespace Blauhaus.Analytics.Tests.Tests.AppInsightsServiceTests.AppInsightsServerServiceTests
{
    public class StartPageViewOperationTests : BaseStartPageViewOperationTests<AnalyticsServerService>
    {
        protected override AnalyticsServerService ConstructSut()
        {
            return new AnalyticsServerService(
                MockConfig.Object,
                MockConsoleLogger.Object,
                MockTelemetryClient.Object,
                MockTelemetryDecorator.Object,
                CurrentBuildConfig);
        }
    }
}