using System;
using Blauhaus.Analytics.Abstractions.Session;
using Microsoft.AspNetCore.Http;

namespace Blauhaus.Analytics.AspNetCore.SessionFactories
{
    public class AspNetCoreWebSessionFactory : IAnalyticsSessionFactory
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AspNetCoreWebSessionFactory(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public IAnalyticsSession CreateSession()
        {
            var currentSession = AnalyticsSession.New;

            if (_httpContextAccessor.HttpContext == null)
            {
                //there is no user
                return currentSession;
            }

            var userId = _httpContextAccessor.HttpContext.Request.Cookies["UserId"];
            if (string.IsNullOrEmpty(userId))
            {
                userId = Guid.NewGuid().ToString();
                _httpContextAccessor.HttpContext.Response.Cookies.Append("UserId", userId, new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddYears(5)
                });
            }

            currentSession.UserId = userId;
            
            return currentSession;
        }
    }
}