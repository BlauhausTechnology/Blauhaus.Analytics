using System;
using System.Collections.Generic;
using Blauhaus.Analytics.Abstractions.Operation;
using Blauhaus.Analytics.Tests.Tests._Base;
using Microsoft.ApplicationInsights.DataContracts;
using Moq;
using NUnit.Framework;

namespace Blauhaus.Analytics.Tests.Tests.AnalyticsServiceTests
{
    public class ContinueOperationTests : BaseAnalyticsServiceTest
    {
        [Test]
        public void IF_no_operation_exists_SHOULD_start_new_one()
        {
            //Act
            var operation = Sut.ContinueOperation(this, "MyOperation");

            //Assert
            Assert.That(operation.Name, Is.EqualTo("MyOperation"));
            Assert.That(Sut.CurrentOperation.Name, Is.EqualTo("MyOperation"));
            Assert.That(Sut.CurrentOperation.Id.Length, Is.EqualTo(Guid.NewGuid().ToString().Length));
            Assert.That(operation.Id.Length, Is.EqualTo(Guid.NewGuid().ToString().Length));
        }

        [Test]
        public void IF_no_operation_exists_and_Operation_is_disposed_SHOULD_track_dependency()
        {
            //Arrange
            var operation = Sut.ContinueOperation(this, "MyOperation", new Dictionary<string, object>{{"key", "1"}});
            MockTelemetryClient.Mock.Verify(x => x.TrackDependency(It.IsAny<DependencyTelemetry>()), Times.Never);

            //Act
            operation.Dispose();
            
            //Assert
            MockTelemetryDecorator.Mock.Verify<DependencyTelemetry>(x => x.DecorateTelemetry(It.IsAny<DependencyTelemetry>(),
                It.IsAny<string>(), It.IsAny<string>(),
                    It.Is<IAnalyticsOperation>(y => 
                        y.Id == operation.Id &&
                        y.Name == "MyOperation"), 
                Sut.CurrentSession, It.Is<Dictionary<string, object>>(y => (string) y["key"] == "1")));
            MockTelemetryClient.Mock.Verify(x => x.TrackDependency(It.Is<DependencyTelemetry>(y => 
                y.Name == "MyOperation")));
            MockConsoleLogger.Mock.Verify(x => x.LogOperation("MyOperation", It.IsAny<TimeSpan>()));
        }

        [Test]
        public void IF_an_operation_already_exists_SHOULD_start_and_return_new_one_with_same_name_but_keep_current_operation_the_same()
        {
            //Arrange
            var firstOperation = Sut.StartOperation(this, "MyFirstOperation");

            //Act
            var result = Sut.ContinueOperation(this, "MySecondOperation");

            //Assert
            Assert.That(result.Name, Is.EqualTo("MyFirstOperation"));
            Assert.That(result.Id.Length, Is.Not.EqualTo(firstOperation.Id));
            Assert.That(Sut.CurrentOperation.Name, Is.EqualTo("MyFirstOperation"));
            Assert.That(Sut.CurrentOperation.Id, Is.EqualTo(firstOperation.Id));
        }

        
        [Test]
        public void IF_an_operation_already_exists__and_new_one_is_disposed_SHOULD_log_dependency_with_new_operation()
        {
            //Arrange
            var firstOperation = Sut.StartOperation(this, "MyFirstOperation");
            var result = Sut.ContinueOperation(this, "MySecondOperation", new Dictionary<string, object>{{"key", "1"}});

            //Act
            result.Dispose();

            //Assert
            MockTelemetryDecorator.Mock.Verify<DependencyTelemetry>(x => x.DecorateTelemetry(It.Is<DependencyTelemetry>(y => 
                y.Name == "MySecondOperation"),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.Is<IAnalyticsOperation>(y => 
                y.Id == firstOperation.Id && y.Name == "MyFirstOperation"), Sut.CurrentSession, It.Is<Dictionary<string, object>>(y => 
                (string) y["key"] == "1")));
            MockTelemetryClient.Mock.Verify(x => x.TrackDependency(It.Is<DependencyTelemetry>(y => 
                y.Name == "MySecondOperation")));
            MockConsoleLogger.Mock.Verify(x => x.LogOperation("MySecondOperation", It.IsAny<TimeSpan>()));
        }


    }
}