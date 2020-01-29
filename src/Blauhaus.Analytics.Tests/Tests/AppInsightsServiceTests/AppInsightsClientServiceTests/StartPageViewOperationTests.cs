using System;
using Blauhaus.Analytics.Abstractions.Operation;
using Blauhaus.Analytics.Client.Service;
using Blauhaus.Analytics.Tests.Tests._Base;
using Microsoft.ApplicationInsights.DataContracts;
using Moq;
using NUnit.Framework;

namespace Blauhaus.Analytics.Tests.Tests.AppInsightsServiceTests.AppInsightsClientServiceTests
{
    [TestFixture]
    public class StartPageViewOperationTests : BaseAnalyticsServiceTest<AnalyticsClientService>
    {
        protected override AnalyticsClientService ConstructSut()
        {
            return new AnalyticsClientService(
                MockConfig.Object,
                MockConsoleLogger.Object,
                MockTelemetryClient.Object,
                CurrentBuildConfig);
        }

        [Test]
        public void SHOULD_set_and_return_CurrentOperation()
        {
            //Act
            var operation = Sut.StartPageViewOperation("MyOperation");

            //Assert
            Assert.That(operation.Name, Is.EqualTo("MyOperation"));
            Assert.That(Sut.CurrentOperation.Name, Is.EqualTo("MyOperation"));
            Assert.That(Sut.CurrentOperation.Id.Length, Is.EqualTo(Guid.NewGuid().ToString().Length));
            Assert.That(operation.Id.Length, Is.EqualTo(Guid.NewGuid().ToString().Length));
        }

        [Test]
        public void WHEN_Operation_is_disposed_SHOULD_track_dependency()
        {
            //Arrange
            var operation = Sut.StartPageViewOperation("MyOperation");
            MockTelemetryClient.Mock.Verify(x => x.TrackPageView(It.IsAny<PageViewTelemetry>()), Times.Never);

            //Act
            operation.Dispose();
            
            //Assert
            MockTelemetryClient.Mock.Verify(x => x.UpdateOperation(It.Is<IAnalyticsOperation>(y => 
                y.Id == operation.Id &&
                y.Name == "MyOperation"), Sut.CurrentSession));
            MockTelemetryClient.Mock.Verify(x => x.TrackPageView(It.Is<PageViewTelemetry>(y => 
                y.Name == "MyOperation")));
            MockConsoleLogger.Mock.Verify(x => x.LogOperation("MyOperation", It.IsAny<TimeSpan>()));
        }
    }
}