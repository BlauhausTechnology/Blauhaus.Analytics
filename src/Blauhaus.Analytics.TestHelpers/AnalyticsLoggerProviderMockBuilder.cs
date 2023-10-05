using Blauhaus.Analytics.Abstractions;
using Blauhaus.Analytics.TestHelpers.MockBuilders;
using Blauhaus.TestHelpers.MockBuilders;
using Moq;

namespace Blauhaus.Analytics.TestHelpers;

public class AnalyticsLoggerProviderMockBuilder : BaseMockBuilder<AnalyticsLoggerProviderMockBuilder, IAnalyticsLoggerProvider>
{
    public AnalyticsLoggerMockBuilder MockLogger { get; }

    public AnalyticsLoggerProviderMockBuilder()
    {
        MockLogger = new AnalyticsLoggerMockBuilder();
        Mock.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(MockLogger.Object);
    }
}