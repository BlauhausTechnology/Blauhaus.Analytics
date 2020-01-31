using System.Collections.Generic;
using Blauhaus.Analytics.Abstractions.Operation;
using Blauhaus.Analytics.Abstractions.Session;
using Blauhaus.Analytics.Common.TelemetryClients;
using Blauhaus.Analytics.Tests.Tests._Base;
using Blauhaus.Common.TestHelpers;
using Microsoft.ApplicationInsights.DataContracts;
using NUnit.Framework;
using NUnit.Framework.Internal.Execution;

namespace Blauhaus.Analytics.Tests.Tests.AppInsightsServiceTests.TelemetryDecoratorTests
{
    public class DecorateTelemetryTests : BaseAnalyticsServiceTest<TelemetryDecorator>
    {
        private MockBuilder<IAnalyticsOperation> _mockCurrentOperation;
        private MockBuilder<IAnalyticsSession> _mockCurrentSession;

        protected override TelemetryDecorator ConstructSut()
        {
            return new TelemetryDecorator(MockConfig.Object);
        }

        public override void Setup()
        {
            base.Setup();
            _mockCurrentOperation = new MockBuilder<IAnalyticsOperation>();
            _mockCurrentSession = new MockBuilder<IAnalyticsSession>();
        }

        [Test]
        public void SHOULD_append_config_values()
        {
            //Arrange
            MockConfig.With(x => x.InstrumentationKey, "instrument");
            MockConfig.With(x => x.RoleName, "Mata Hari");

            //Act
            var result = Sut.DecorateTelemetry(new EventTelemetry("event"), 
                new MockBuilder<IAnalyticsOperation>().Object, new MockBuilder<IAnalyticsSession>().Object, new Dictionary<string, object>());

            //Assert
            Assert.That(result.Context.Cloud.RoleName, Is.EqualTo("Mata Hari"));
            Assert.That(result.Context.InstrumentationKey, Is.EqualTo("instrument"));
        }
    }
}