﻿using System;
using System.Collections.Generic;
using Blauhaus.Analytics.Abstractions.Operation;
using Blauhaus.Analytics.Tests.Tests.Base;
using Microsoft.ApplicationInsights.DataContracts;
using Moq;
using NUnit.Framework;

namespace Blauhaus.Analytics.Tests.Tests.AnalyticsServiceTests
{
    public class StartPageViewOperationTests : BaseAnalyticsServiceTest
    {
        [Test]
        public void SHOULD_set_and_return_CurrentOperation()
        {
            //Act
            var operation = Sut.StartPageViewOperation(this, "MyOperation");

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
            var operation = Sut.StartPageViewOperation(this, "MyOperation");
            MockTelemetryClient.Mock.Verify(x => x.TrackPageView(It.IsAny<PageViewTelemetry>()), Times.Never);
            MockTelemetryDecorator.Where_Decorate_returns(new PageViewTelemetry("Decorated"));

            //Act
            operation.Dispose();
            
            //Assert
            MockTelemetryDecorator.Mock.Verify<PageViewTelemetry>(x => x.DecorateTelemetry(
                It.Is<PageViewTelemetry>(y => y.Name == "MyOperation"),
                It.IsAny<string>(),
                "WHEN_Operation_is_disposed_SHOULD_track_dependency",
                It.Is<IAnalyticsOperation>(y => y.Name == "MyOperation"), 
                Sut.CurrentSession, 
                It.IsAny<Dictionary<string, object>>()));
            MockTelemetryClient.Mock.Verify(x => x.TrackPageView(It.Is<PageViewTelemetry>(y => y.Name == "Decorated")));
            MockConsoleLogger.Mock.Verify(x => x.LogOperation("MyOperation", It.IsAny<TimeSpan>()));
        }

        [Test]
        public void WHEN_Operation_is_replaced_SHOULD_track_dependency()
        {
            //Arrange
            Sut.StartPageViewOperation(this, "MyOperation");
            MockTelemetryClient.Mock.Verify(x => x.TrackPageView(It.IsAny<PageViewTelemetry>()), Times.Never);
            MockTelemetryDecorator.Where_Decorate_returns(new PageViewTelemetry("Decorated"));

            //Act
            Sut.StartPageViewOperation(this, "New");
            
            //Assert
            MockTelemetryDecorator.Mock.Verify<PageViewTelemetry>(x => x.DecorateTelemetry(
                It.Is<PageViewTelemetry>(y => y.Name == "MyOperation"),
                It.IsAny<string>(),
                "WHEN_Operation_is_replaced_SHOULD_track_dependency",
                It.Is<IAnalyticsOperation>(y => y.Name == "MyOperation"), 
                Sut.CurrentSession, 
                It.IsAny<Dictionary<string, object>>()));
            MockTelemetryClient.Mock.Verify(x => x.TrackPageView(It.Is<PageViewTelemetry>(y => y.Name == "Decorated")));
            MockConsoleLogger.Mock.Verify(x => x.LogOperation("MyOperation", It.IsAny<TimeSpan>()));
        }

        [Test]
        public void IF_PageName_is_empty_SHOULD_use_class_name()
        {
            //Arrange
            var operation = Sut.StartPageViewOperation(this);
            MockTelemetryClient.Mock.Verify(x => x.TrackPageView(It.IsAny<PageViewTelemetry>()), Times.Never);
            MockTelemetryDecorator.Where_Decorate_returns(new PageViewTelemetry("Decorated"));

            //Act
            operation.Dispose();
            
            //Assert
            MockTelemetryDecorator.Mock.Verify<PageViewTelemetry>(x => x.DecorateTelemetry(
                It.Is<PageViewTelemetry>(y => y.Name == GetType().Name),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<IAnalyticsOperation>(), 
                Sut.CurrentSession, 
                It.IsAny<Dictionary<string, object>>()));
            MockTelemetryClient.Mock.Verify(x => x.TrackPageView(It.Is<PageViewTelemetry>(y => y.Name == "Decorated")));
            MockConsoleLogger.Mock.Verify(x => x.LogOperation(GetType().Name, It.IsAny<TimeSpan>()));
        }
         
    }
}