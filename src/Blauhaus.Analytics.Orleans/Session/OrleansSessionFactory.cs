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
            if (_requestContext.TryGet(AnalyticsHeaders.Session.Id, out var sessionId))
            {
                var session = AnalyticsSession.FromExisting((string) sessionId);

                if (_requestContext.TryGet(AnalyticsHeaders.Session.UserId, out var userId)) session.UserId = (string)userId;
                if (_requestContext.TryGet(AnalyticsHeaders.Session.DeviceId, out var deviceId)) session.DeviceId = (string)deviceId;
                if (_requestContext.TryGet(AnalyticsHeaders.Session.AppVersion, out var appVersion)) session.AppVersion = (string)appVersion;
                if (_requestContext.TryGet(AnalyticsHeaders.Session.AccountId, out var accountId)) session.AccountId = (string)accountId;
                if (_requestContext.TryGet(AnalyticsHeaders.Operation.Id, out var operationId)) session.SetProperty("OperationId", operationId.ToString());
                if (_requestContext.TryGet(AnalyticsHeaders.Operation.Name, out var operationName)) session.SetProperty("OperationName", operationName.ToString());
                
                return session;
            }

            return AnalyticsSession.Empty;

        }
    }
}