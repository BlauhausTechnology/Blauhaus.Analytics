using System;
using Blauhaus.Analytics.Tests.Tests._Base;
using NUnit.Framework;

namespace Blauhaus.Analytics.Tests.Tests.AnalyticsServiceTests
{
    public class ClearCurrentSessionTestss : BaseAnalyticsServiceTest
    {
        [Test]
        public void IF_no_new_session_Id_is_provided_SHOULD_reset_with_old_sessionId()
        {
            //Arrange
            Sut.StartOperation(this, "MyOperation");
            var originalSessionId = Sut.CurrentSession.Id;

            //Act
            Sut.ResetCurrentSession();

            //Assert
            Assert.That(Sut.CurrentOperation, Is.Null);
            Assert.That(Sut.CurrentSession.Id, Is.EqualTo(originalSessionId));
        }

        [Test]
        public void IF_a_new_session_Id_is_provided_SHOULD_reset_with_new_sessionId()
        {
            //Arrange
            Sut.StartOperation(this, "MyOperation");

            //Act
            Sut.ResetCurrentSession("newSessionId");

            //Assert
            Assert.That(Sut.CurrentOperation, Is.Null);
            Assert.That(Sut.CurrentSession.Id, Is.EqualTo("newSessionId"));
        }


    }
}