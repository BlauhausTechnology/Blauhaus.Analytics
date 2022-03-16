using Blauhaus.Analytics.Abstractions;
using Blauhaus.Analytics.Abstractions.Config;
using Blauhaus.Analytics.Abstractions.Http;
using Blauhaus.Analytics.Abstractions.Session;
using Blauhaus.Analytics.Common.Service;
using Blauhaus.Analytics.Common.Telemetry;
using Blauhaus.Analytics.Console.ConsoleLoggers;
using Blauhaus.Common.ValueObjects.BuildConfigs;

namespace Blauhaus.Analytics.Orleans
{
    public class OrleansAnalyticsService : AnalyticsService
    {
        private readonly IAnalyticsContext _analyticsContext;

        public OrleansAnalyticsService(
            IApplicationInsightsConfig config, 
            IConsoleLogger consoleLogger, 
            ITelemetryClientProxy telemetryClient, 
            ITelemetryDecorator telemetryDecorator, 
            IBuildConfig currentBuildConfig, 
            IAnalyticsSessionFactory sessionFactory,
            IAnalyticsContext analyticsContext) 
                : base(config, consoleLogger, telemetryClient, telemetryDecorator, currentBuildConfig, sessionFactory)
        {
            _analyticsContext = analyticsContext;
        }

        protected override void HandleNewRequest(AnalyticsSession session, string operationName, string operationId)
        {
            _analyticsContext.SetValue(AnalyticsHeaders.Operation.Name, operationName);
            _analyticsContext.SetValue(AnalyticsHeaders.Operation.Id, operationId);
            
            _analyticsContext.SetValue(AnalyticsHeaders.Session.Id, session.Id);
            _analyticsContext.SetValue(AnalyticsHeaders.Session.AccountId, session.AccountId ?? string.Empty);
            _analyticsContext.SetValue(AnalyticsHeaders.Session.UserId, session.UserId ?? string.Empty);
            _analyticsContext.SetValue(AnalyticsHeaders.Session.DeviceId, session.DeviceId ?? string.Empty);
            _analyticsContext.SetValue(AnalyticsHeaders.Session.AppVersion, session.AppVersion ?? string.Empty);
        }
    }
}