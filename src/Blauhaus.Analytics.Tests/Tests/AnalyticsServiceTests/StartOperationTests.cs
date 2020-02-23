using System;
using System.Collections.Generic;
using Blauhaus.Analytics.Abstractions.Operation;
using Blauhaus.Analytics.Tests.Tests._Base;
using Microsoft.ApplicationInsights.DataContracts;
using Moq;
using NUnit.Framework;

namespace Blauhaus.Analytics.Tests.Tests.AnalyticsServiceTests
{
    public class StartOperationTests : BaseAnalyticsServiceTest
    {
        [Test]
        public void SHOULD_set_and_return_CurrentOperation()
        {
            //Act
            var operation = Sut.StartOperation(this, "MyOperation");

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
            var operation = Sut.StartOperation(this, "MyOperation", new Dictionary<string, object>
            {
                {"key", "1" }
            });
            MockTelemetryClient.Mock.Verify(x => x.TrackDependency(It.IsAny<DependencyTelemetry>()), Times.Never);

            //Act
            operation.Dispose();
            
            //Assert
            MockTelemetryDecorator.Mock.Verify<DependencyTelemetry>(x => x.DecorateTelemetry(It.IsAny<DependencyTelemetry>(), 
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.Is<IAnalyticsOperation>(y => 
                    y.Id == operation.Id &&
                    y.Name == "MyOperation"), 
                Sut.CurrentSession, It.Is<Dictionary<string, object>>(y => (string) y["key"] == "1")));
            MockTelemetryClient.Mock.Verify(x => x.TrackDependency(It.Is<DependencyTelemetry>(y => 
                y.Name == "MyOperation")));
            MockConsoleLogger.Mock.Verify(x => x.LogOperation("MyOperation", It.IsAny<TimeSpan>()));
        }

    }
}