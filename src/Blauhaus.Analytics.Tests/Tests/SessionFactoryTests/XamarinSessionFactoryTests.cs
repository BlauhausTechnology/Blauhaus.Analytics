using System;
using Blauhaus.Analytics.AspNetCore.SessionFactories;
using Blauhaus.Analytics.Tests.Tests._Base;
using Blauhaus.Analytics.Xamarin.SessionFactories;
using Blauhaus.Common.ValueObjects.DeviceType;
using Blauhaus.Common.ValueObjects.RuntimePlatforms;
using NUnit.Framework;

namespace Blauhaus.Analytics.Tests.Tests.SessionFactoryTests
{
    public class XamarinSessionFactoryTests : BaseAnalyticsTest<XamarinSessionFactory>
    {
        protected override XamarinSessionFactory ConstructSut()
        {
            return new XamarinSessionFactory(MockDeviceInfoService.Object, MockApplicationInfoService.Object);
        }

        [Test]
        public void SHOULD_return_same_session_each_time()
        {
            //Act
            var result1 = Sut.CreateSession();
            var result2 = Sut.CreateSession();

            //Assert
            Assert.That(result1.Id.Length, Is.EqualTo(Guid.NewGuid().ToString().Length));
            Assert.That(result2.Id.Length, Is.EqualTo(Guid.NewGuid().ToString().Length));
            Assert.That(result1.Id, Is.EqualTo(result2.Id));
        }

        [Test]
        public void SHOULD_populate_app_version_info()
        {
            //Arrange
            MockApplicationInfoService.With(x => x.CurrentVersion, "1.2.2");

            //Act
            var result = Sut.CreateSession();

            //Assert
            Assert.That(result.AppVersion, Is.EqualTo("1.2.2"));
        }
        
        [Test]
        public void SHOULD_populate_device_info()
        {
            //Arrange
            MockDeviceInfoService
                .With(x => x.DeviceUniqueIdentifier, "deviceId")
                .With(x => x.Manufacturer, "Samsinging")
                .With(x => x.Model, "E-250")
                .With(x => x.OperatingSystemVersion, "3.4")
                .With(x => x.Type, DeviceType.Phone)
                .With(x => x.Platform, RuntimePlatform.UWP);

            //Act
            var result = Sut.CreateSession();

            //Assert
            Assert.That(result.DeviceId, Is.EqualTo("deviceId"));
            Assert.That(result.Manufacturer, Is.EqualTo("Samsinging"));
            Assert.That(result.Model, Is.EqualTo("E-250"));
            Assert.That(result.OperatingSystemVersion, Is.EqualTo("3.4"));
            Assert.That(result.DeviceType.Value, Is.EqualTo("Phone"));
            Assert.That(result.Platform.Value, Is.EqualTo("UWP"));
        }
    }
}