using System.Collections.Generic;
using Blauhaus.Analytics.Abstractions.Operation;
using Blauhaus.Analytics.Abstractions.Session;
using Blauhaus.Analytics.Common.Telemetry;
using Blauhaus.Analytics.Tests.Tests._Base;
using Blauhaus.Common.TestHelpers;
using Microsoft.ApplicationInsights.DataContracts;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Blauhaus.Analytics.Tests.Tests.TelemetryDecoratorTests
{
    public class DecorateTelemetryTests : BaseAnalyticsTest<TelemetryDecorator>
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
            var result = Sut.DecorateTelemetry(new EventTelemetry("event"), "Class Name", "Method Name",
                new MockBuilder<IAnalyticsOperation>().Object, new MockBuilder<IAnalyticsSession>().Object, new Dictionary<string, object>());

            //Assert
            Assert.That(result.Context.Cloud.RoleName, Is.EqualTo("Mata Hari"));
            Assert.That(result.Context.InstrumentationKey, Is.EqualTo("instrument"));
        }
        
        [Test]
        public void SHOULD_set_class_and_method_names()
        {
            //Arrange
            MockConfig.With(x => x.InstrumentationKey, "instrument");
            MockConfig.With(x => x.RoleName, "Mata Hari");

            //Act
            var result = Sut.DecorateTelemetry(new EventTelemetry("event"), "Class Name", "Method Name",
                new MockBuilder<IAnalyticsOperation>().Object, new MockBuilder<IAnalyticsSession>().Object, new Dictionary<string, object>());

            //Assert
            Assert.That(result.Properties["Class"], Is.EqualTo("Class Name"));
            Assert.That(result.Properties["Method"], Is.EqualTo("Method Name"));
        }
        
        [Test]
        public void IF_operation_is_provided_SHOULD_add_details()
        {
            //Arrange
            var currentOperation = new MockBuilder<IAnalyticsOperation>()
                .With(x => x.Name, "Op Name")
                .With(x => x.Id, "Op Id").Object;

            //Act
            var result = Sut.DecorateTelemetry(new EventTelemetry("event"), "Class Name", "Method Name",
               currentOperation, new MockBuilder<IAnalyticsSession>().Object, new Dictionary<string, object>());

            //Assert
            Assert.That(result.Context.Operation.Name, Is.EqualTo("Op Name"));
            Assert.That(result.Context.Operation.Id, Is.EqualTo("Op Id"));
        }

        [Test]
        public void WHEN_session_values_are_provided_SHOULD_add_them()
        {
            //Arrange
            var currentSession = new MockBuilder<IAnalyticsSession>()
                .With(x => x.AppVersion, "ver")
                .With(x => x.AccountId, "acc")
                .With(x => x.DeviceId, "dev")
                .With(x => x.UserId, "use")
                .With(x => x.Id, "sessionId")
                .With(x => x.Properties, new Dictionary<string, string>
                {
                    {"key", "value" }
                });

            //Act
            var result = Sut.DecorateTelemetry(new EventTelemetry("event"), "Class Name", "Method Name",
                new MockBuilder<IAnalyticsOperation>().Object, currentSession.Object, new Dictionary<string, object>());

            //Assert
            Assert.That(result.Context.Session.Id, Is.EqualTo("sessionId"));
            Assert.That(result.Context.Component.Version, Is.EqualTo("ver"));
            Assert.That(result.Context.User.AccountId, Is.EqualTo("acc"));
            Assert.That(result.Context.User.AuthenticatedUserId, Is.EqualTo("use"));
            Assert.That(result.Context.Device.Id, Is.EqualTo("dev"));
            Assert.That(result.Properties["key"], Is.EqualTo("value"));
        }

        [Test]
        public void WHEN_properties_are_provided_SHOULD_add_them()
        {
            //Arrange
            var properties = new Dictionary<string, object>
            {
                {"key", "value" }
            };

            //Act
            var result = Sut.DecorateTelemetry(new EventTelemetry("event"), "Class Name", "Method Name",
                new MockBuilder<IAnalyticsOperation>().Object, new MockBuilder<IAnalyticsSession>().Object, properties);

            //Assert
            Assert.That(result.Properties["key"], Is.EqualTo("value"));
        }

        [Test]
        public void WHEN_properties_are_objects_SHOULD_Jsonify_them()
        {
            //Arrange
            var objectProperty = new
            {
                Name = "Adrian"
            };
            var properties = new Dictionary<string, object>
            {
                {"key", objectProperty }
            };

            //Act
            var result = Sut.DecorateTelemetry(new EventTelemetry("event"), "Class Name", "Method Name",
                new MockBuilder<IAnalyticsOperation>().Object, new MockBuilder<IAnalyticsSession>().Object, properties);

            //Assert
            Assert.That(result.Properties["key"], Is.EqualTo(JsonConvert.SerializeObject(objectProperty)));
        }

        [Test]
        public void WHEN_metrics_are_provided_SHOULD_add_them()
        {
            //Arrange
            var metrics = new Dictionary<string, double>
            {
                {"key", 122}
            };

            //Act
            var result = Sut.DecorateTelemetry(new EventTelemetry("event"), "Class Name", "Method Name",
                new MockBuilder<IAnalyticsOperation>().Object, new MockBuilder<IAnalyticsSession>().Object, new Dictionary<string, object>(), metrics);

            //Assert
            Assert.That(result.Metrics["key"], Is.EqualTo(122));
        }
    }
}