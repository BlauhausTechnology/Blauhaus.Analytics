using System.Collections.Generic;
using Blauhaus.Analytics.Abstractions.Operation;
using Blauhaus.Analytics.Abstractions.Session;
using Blauhaus.Analytics.Common.TelemetryClients;
using Blauhaus.Common.TestHelpers;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Moq;

namespace Blauhaus.Analytics.Tests.MockBuilders
{
    public class TelemetryDecoratorMockBuilder : BaseMockBuilder<TelemetryDecoratorMockBuilder, ITelemetryDecorator>
    {

        public TelemetryDecoratorMockBuilder()
        {
        }

        public TelemetryDecoratorMockBuilder Where_Decorate_returns<TTelemetry>(TTelemetry telemetry) 
            where TTelemetry : ITelemetry, ISupportProperties
        {
            Mock.Setup(x => x.DecorateTelemetry(
                It.IsAny<TTelemetry>(), 
                It.IsAny<IAnalyticsOperation>(), 
                It.IsAny<IAnalyticsSession>(), 
                It.IsAny<Dictionary<string, object>>()))
                .Returns(telemetry);
            return this;
        }

        
        public TelemetryDecoratorMockBuilder Where_Decorate_with_metrics_returns<TTelemetry>(TTelemetry telemetry) 
            where TTelemetry : ITelemetry, ISupportProperties, ISupportMetrics
        {
            Mock.Setup(x => x.DecorateTelemetry(
                It.IsAny<TTelemetry>(), 
                It.IsAny<IAnalyticsOperation>(), 
                It.IsAny<IAnalyticsSession>(), 
                It.IsAny<Dictionary<string, object>>(),
                It.IsAny<Dictionary<string, double>>()))
                .Returns(telemetry);
            return this;
        }
    }
}