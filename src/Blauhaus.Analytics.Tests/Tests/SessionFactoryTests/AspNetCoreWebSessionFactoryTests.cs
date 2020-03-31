using System;
using Blauhaus.Analytics.AspNetCore.SessionFactories;
using Blauhaus.Analytics.Tests.Tests._Base;
using Blauhaus.TestHelpers.MockBuilders;
using Microsoft.AspNetCore.Http;
using NUnit.Framework;

namespace Blauhaus.Analytics.Tests.Tests.SessionFactoryTests
{
    public class AspNetCoreWebSessionFactoryTests : BaseAnalyticsTest<AspNetCoreWebSessionFactory>
    {
        private MockBuilder<IHttpContextAccessor> _mockHttpContextAccessor;
        private HttpContext _httpContext;

        protected override AspNetCoreWebSessionFactory ConstructSut()
        {
            return new AspNetCoreWebSessionFactory(_mockHttpContextAccessor.Object);
        }

        public override void Setup()
        {
            base.Setup();
            _httpContext  = new DefaultHttpContext();
            _mockHttpContextAccessor = new MockBuilder<IHttpContextAccessor>();
        }

        [Test]
        public void IF_HttpContext_is_null_SHOULD_return_new_session_each_time()
        {
            //Arrange
            _httpContext = null;
            _mockHttpContextAccessor.With(x => x.HttpContext, _httpContext);

            //Act
            var result1 = Sut.CreateSession();
            var result2 = Sut.CreateSession();

            //Assert
            Assert.That(result1.Id.Length, Is.EqualTo(Guid.NewGuid().ToString().Length));
            Assert.That(result1.UserId, Is.Null);
            Assert.That(result2.Id.Length, Is.EqualTo(Guid.NewGuid().ToString().Length));
            Assert.That(result2.UserId, Is.Null);
            Assert.That(result1.Id, Is.Not.EqualTo(result2.Id));
        }

        //todo use HttpContextProxy to test

        //[Test]
        //public void IF_UserId_cookie_is_set_SHOULD_add_to_session()
        //{
        //    //Arrange
        //    var userId = Guid.NewGuid().ToString();
        //    _httpContext = new DefaultHttpContext();
        //    _httpContext.Request.Cookies.Append(new KeyValuePair<string, string>("UserId", userId));
        //    _mockHttpContextAccessor.With(x => x.HttpContext, _httpContext);

        //    //Act
        //    var result1 = Sut.CreateSession();
        //    var result2 = Sut.CreateSession();

        //    //Assert
        //    Assert.That(result1.Id.Length, Is.EqualTo(Guid.NewGuid().ToString().Length));
        //    Assert.That(result1.UserId, Is.EqualTo(userId));
        //    Assert.That(result2.Id.Length, Is.EqualTo(Guid.NewGuid().ToString().Length));
        //    Assert.That(result2.UserId, Is.EqualTo(userId));
        //    Assert.That(result1.Id, Is.Not.EqualTo(result2.Id));
        //}

        //[Test]
        //public void IF_HttpContext_User_is_not_null_SHOULD_return_new_session_each_time_with_userId()
        //{
        //    //Arrange
        //    var identity = new ClaimsIdentity(new List<Claim>(), "Bearer");
        //    _httpContext.User = ;

        //    //Act
        //    var result1 = Sut.CreateSession();
        //    var result2 = Sut.CreateSession();

        //    //Assert
        //    Assert.That(result1.Id.Length, Is.EqualTo(Guid.NewGuid().ToString().Length));
        //    Assert.That(result1.Id.Length, Is.EqualTo(Guid.NewGuid().ToString().Length));
        //    Assert.That(result2.Id.Length, Is.EqualTo(Guid.NewGuid().ToString().Length));
        //    Assert.That(result1.Id, Is.Not.EqualTo(result2.Id));
        //}
    }
}