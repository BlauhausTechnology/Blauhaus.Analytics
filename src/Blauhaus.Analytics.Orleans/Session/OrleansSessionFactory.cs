using Blauhaus.Analytics.Abstractions;
using Blauhaus.Analytics.Abstractions.Http;
using Blauhaus.Analytics.Abstractions.Session;

namespace Blauhaus.Analytics.Orleans.Session
{
    public class OrleansSessionFactory : IAnalyticsSessionFactory
    {
        private readonly IAnalyticsContext _requestContext;

        public OrleansSessionFactory(IAnalyticsContext requestContext)
        {
            _requestContext = requestContext;
        }
        
        public IAnalyticsSession CreateSession()
        {
            if (_requestContext.TryGetValue(AnalyticsHeaders.Session.Id, out var sessionId))
            {
                var session = AnalyticsSession.FromExisting((string) sessionId);

                if (_requestContext.TryGetValue(AnalyticsHeaders.Session.UserId, out var userId)) session.UserId = (string)userId;
                if (_requestContext.TryGetValue(AnalyticsHeaders.Session.DeviceId, out var deviceId)) session.DeviceId = (string)deviceId;
                if (_requestContext.TryGetValue(AnalyticsHeaders.Session.AppVersion, out var appVersion)) session.AppVersion = (string)appVersion;
                if (_requestContext.TryGetValue(AnalyticsHeaders.Session.AccountId, out var accountId)) session.AccountId = (string)accountId;
                if (_requestContext.TryGetValue(AnalyticsHeaders.Operation.Id, out var operationId)) session.SetProperty("OperationId", operationId.ToString());
                if (_requestContext.TryGetValue(AnalyticsHeaders.Operation.Name, out var operationName)) session.SetProperty("OperationName", operationName.ToString());
                
                return session;
            }

            return AnalyticsSession.Empty;

        }
    }
}