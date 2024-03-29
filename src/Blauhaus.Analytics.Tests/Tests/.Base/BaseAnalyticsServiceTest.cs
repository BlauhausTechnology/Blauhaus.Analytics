﻿using Blauhaus.Analytics.Common.Service;

namespace Blauhaus.Analytics.Tests.Tests.Base
{
    public abstract class BaseAnalyticsServiceTest : BaseAnalyticsTest<AnalyticsService> 
    {
        
        protected override AnalyticsService ConstructSut()
        {
            return new AnalyticsService(
                MockConfig.Object,
                MockConsoleLogger.Object,
                MockTelemetryClient.Object,
                MockTelemetryDecorator.Object,
                CurrentBuildConfig,
                MockAnalyticsSessionFactory.Object);
        }


    }
}
