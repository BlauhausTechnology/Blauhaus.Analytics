using System;
using Blauhaus.Analytics.AspNetCore.SessionFactories;
using Blauhaus.Analytics.Tests.Tests.Base;
using NUnit.Framework;

namespace Blauhaus.Analytics.Tests.Tests.SessionFactoryTests
{
    public class AspNetCoreApiSessionFactoryTests : BaseAnalyticsTest<AspNetCoreApiSessionFactory>
    {
        protected override AspNetCoreApiSessionFactory ConstructSut()
        {
            return new AspNetCoreApiSessionFactory();
        }

        [Test]
        public void SHOULD_return_new_session_each_time()
        {
            //Act
            var result1 = Sut.CreateSession();
            var result2 = Sut.CreateSession();

            //Assert
            Assert.That(result1.Id.Length, Is.EqualTo(Guid.NewGuid().ToString().Length));
            Assert.That(result2.Id.Length, Is.EqualTo(Guid.NewGuid().ToString().Length));
            Assert.That(result1.Id, Is.Not.EqualTo(result2.Id));

        }
    }
}