using System;
using System.Collections.Generic;
using Blauhaus.Analytics.Abstractions.Operation;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Analytics.Abstractions.Session;
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
                MockTelemetryDecorator.Object,
                CurrentBuildConfig,
                MockDeviceInfoService.Object,
                MockApplicationInfoService.Object);
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
            MockTelemetryDecorator.Where_Decorate_with_metrics_returns(new PageViewTelemetry("Decorated"));

            //Act
            operation.Dispose();
            
            //Assert
            MockTelemetryDecorator.Mock.Verify(x => x.DecorateTelemetry(
                It.Is<PageViewTelemetry>(y => y.Name == "MyOperation"),
                It.Is<IAnalyticsOperation>(y => y.Name == "MyOperation"), 
                Sut.CurrentSession, 
                It.IsAny<Dictionary<string, object>>(), It.IsAny<Dictionary<string, double>>()));
            MockTelemetryClient.Mock.Verify(x => x.TrackPageView(It.Is<PageViewTelemetry>(y => y.Name == "Decorated")));
            MockConsoleLogger.Mock.Verify(x => x.LogOperation("MyOperation", It.IsAny<TimeSpan>()));
        }
        
        [Test]
        public void SHOULD_trace_pageview_started_before_disposed()
        {
            //Arrange
            MockTelemetryDecorator.Where_Decorate_returns(new TraceTelemetry("Decorated"));

            //Act
            Sut.StartPageViewOperation("MyOperation");

            //Assert
            MockTelemetryDecorator.Mock.Verify(x => x.DecorateTelemetry(
                It.Is<TraceTelemetry>(y => y.Message == "MyOperation started"),
                It.Is<IAnalyticsOperation>(y => y.Name == "MyOperation"), 
                Sut.CurrentSession, 
                It.IsAny<Dictionary<string, object>>()));
            MockTelemetryClient.Mock.Verify(x => x.TrackTrace(It.Is<TraceTelemetry>(y => y.Message == "Decorated")));
            MockConsoleLogger.Mock.Verify(x => x.LogTrace("MyOperation started", LogSeverity.Verbose, It.IsAny<Dictionary<string, string>?>()));
        }

    }
}