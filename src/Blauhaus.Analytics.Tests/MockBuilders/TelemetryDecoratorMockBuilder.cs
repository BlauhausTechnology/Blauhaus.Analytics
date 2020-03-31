using System.Collections.Generic;
using Blauhaus.Analytics.Abstractions.Operation;
using Blauhaus.Analytics.Abstractions.Session;
using Blauhaus.Analytics.Common.Telemetry;
using Blauhaus.Common.TestHelpers;
using Blauhaus.Common.TestHelpers.MockBuilders;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Moq;

namespace Blauhaus.Analytics.Tests.MockBuilders
{
    public class TelemetryDecoratorMockBuilder : BaseMockBuilder<TelemetryDecoratorMockBuilder, ITelemetryDecorator>
    {
        public TelemetryDecoratorMockBuilder Where_Decorate_returns<TTelemetry>(TTelemetry telemetry) 
            where TTelemetry : ITelemetry, ISupportProperties
        {
            Mock.Setup(x => x.DecorateTelemetry(
                It.IsAny<TTelemetry>(), 
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<IAnalyticsOperation>(), 
                It.IsAny<IAnalyticsSession>(), 
                It.IsAny<Dictionary<string, object>>()))
                .Returns(telemetry);
            return this;
        }

    }
}