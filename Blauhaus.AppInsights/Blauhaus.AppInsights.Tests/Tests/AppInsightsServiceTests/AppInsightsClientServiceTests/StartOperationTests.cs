using Blauhaus.AppInsights.Client.Service;
using Blauhaus.AppInsights.Tests.Tests.AppInsightsServiceTests._BaseTests;
using NUnit.Framework;

namespace Blauhaus.AppInsights.Tests.Tests.AppInsightsServiceTests.AppInsightsClientServiceTests
{
    [TestFixture]
    public class StartOperationTests : BaseStartOperationTests<AppInsightsClientService>
    {
        protected override AppInsightsClientService ConstructSut()
        {
            return new AppInsightsClientService(
                MockConfig.Object,
                MockConsoleLogger.Object,
                MockTelemetryClient.Object);
        }
    }
}