using System;
using Blauhaus.Analytics.Abstractions.Operation;
using Blauhaus.Analytics.Client.Service;
using Blauhaus.Analytics.Console.Service;
using Blauhaus.Analytics.Tests.Tests._Base;
using Microsoft.ApplicationInsights.DataContracts;
using Moq;
using NUnit.Framework;

namespace Blauhaus.Analytics.Tests.Tests.AppInsightsServiceTests.ConsoleLoggerServiceTests
{
    [TestFixture]
    public class StartPageViewOperationTests : BaseAppInsightsTest<ConsoleLoggerService>
    {
        protected override ConsoleLoggerService ConstructSut()
        {
            return new ConsoleLoggerService(
                MockConsoleLogger.Object);
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
            MockConsoleLogger.Mock.Verify(x => x.LogOperation("MyOperation", It.IsAny<TimeSpan>()), Times.Never);

            //Act
            operation.Dispose();
            
            //Assert
            MockConsoleLogger.Mock.Verify(x => x.LogOperation("MyOperation", It.IsAny<TimeSpan>()));
        }
    }
}