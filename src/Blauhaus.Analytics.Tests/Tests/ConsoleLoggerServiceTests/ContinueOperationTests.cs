using System;
using Blauhaus.Analytics.Console.Service;
using Blauhaus.Analytics.Tests.Tests._Base;
using Microsoft.ApplicationInsights.DataContracts;
using Moq;
using NUnit.Framework;

namespace Blauhaus.Analytics.Tests.Tests.ConsoleLoggerServiceTests
{
    public class ContinueOperationTests : BaseAnalyticsTest<ConsoleLoggerService>
    {
        protected override ConsoleLoggerService ConstructSut()
        {
            return new ConsoleLoggerService(
                CurrentBuildConfig,
                MockConsoleLogger.Object);
        }

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
            var operation = Sut.ContinueOperation(this, "MyOperation");
            MockTelemetryClient.Mock.Verify(x => x.TrackDependency(It.IsAny<DependencyTelemetry>()), Times.Never);

            //Act
            operation.Dispose();
            
            //Assert
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
            var result = Sut.ContinueOperation(this, "MySecondOperation");

            //Act
            result.Dispose();

            //Assert
            MockConsoleLogger.Mock.Verify(x => x.LogOperation("MySecondOperation", It.IsAny<TimeSpan>()));
        }

    }
}