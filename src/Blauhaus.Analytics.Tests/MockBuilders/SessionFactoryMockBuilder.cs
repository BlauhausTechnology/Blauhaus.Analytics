using System;
using Blauhaus.Analytics.Abstractions.Session;
using Blauhaus.Common.TestHelpers;
using Moq;

namespace Blauhaus.Analytics.Tests.MockBuilders
{
    public class SessionFactoryMockBuilder : BaseMockBuilder<SessionFactoryMockBuilder, IAnalyticsSessionFactory>
    {
        public SessionFactoryMockBuilder()
        {
            Where_GetSessionAsync_returns(Guid.NewGuid().ToString());
        }

        public SessionFactoryMockBuilder Where_GetSessionAsync_returns(string sessionId)
        {
            Mock.Setup(x => x.GetSessionIdAsync()).ReturnsAsync(sessionId);
            return this;
        }
    }
}