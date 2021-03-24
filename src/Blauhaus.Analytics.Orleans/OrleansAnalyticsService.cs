using Blauhaus.Analytics.Abstractions.Config;
using Blauhaus.Analytics.Abstractions.Http;
using Blauhaus.Analytics.Abstractions.Session;
using Blauhaus.Analytics.Common.Service;
using Blauhaus.Analytics.Common.Telemetry;
using Blauhaus.Analytics.Console.ConsoleLoggers;
using Blauhaus.Analytics.Orleans.Context;
using Blauhaus.Common.ValueObjects.BuildConfigs;

namespace Blauhaus.Analytics.Orleans
{
    public class OrleansAnalyticsService : AnalyticsService
    {
        private readonly IOrleansRequestContext _requestContext;

        public OrleansAnalyticsService(
            IApplicationInsightsConfig config, 
            IConsoleLogger consoleLogger, 
            ITelemetryClientProxy telemetryClient, 
            ITelemetryDecorator telemetryDecorator, 
            IBuildConfig currentBuildConfig, 
            IAnalyticsSessionFactory sessionFactory,
            IOrleansRequestContext requestContext) 
                : base(config, consoleLogger, telemetryClient, telemetryDecorator, currentBuildConfig, sessionFactory)
        {
            _requestContext = requestContext;
        }

        protected override void HandleNewRequest(AnalyticsSession session, string operationName, string operationId)
        {
            _requestContext.Set(AnalyticsHeaders.Operation.Name, operationName);
            _requestContext.Set(AnalyticsHeaders.Operation.Id, operationId);
            
            _requestContext.Set(AnalyticsHeaders.Session.Id, session.Id);
            _requestContext.Set(AnalyticsHeaders.Session.AccountId, session.AccountId);
            _requestContext.Set(AnalyticsHeaders.Session.UserId, session.UserId);
            _requestContext.Set(AnalyticsHeaders.Session.DeviceId, session.DeviceId);
            _requestContext.Set(AnalyticsHeaders.Session.AppVersion, session.AppVersion);
        }
    }
}