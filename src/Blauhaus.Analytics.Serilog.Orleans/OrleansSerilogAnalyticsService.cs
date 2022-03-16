using Blauhaus.Analytics.Abstractions;
using Blauhaus.Analytics.Abstractions.Http;
using Blauhaus.Analytics.Abstractions.Operation;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Analytics.Abstractions.Session;
using Serilog;

namespace Blauhaus.Analytics.Serilog.Orleans
{
    public class OrleansSerilogAnalyticsService : IAnalyticsService
    {
        private IAnalyticsOperation? _currentOperation;
        private IAnalyticsSession? _currentSession;

        private readonly IAnalyticsSessionFactory _sessionFactory;
        private readonly IAnalyticsContext _analyticsContext;

        public OrleansSerilogAnalyticsService(
            IAnalyticsSessionFactory sessionFactory,
            IAnalyticsContext analyticsContext)
        {
            _sessionFactory = sessionFactory;
            _analyticsContext = analyticsContext;
        }


        public IAnalyticsOperation? CurrentOperation
        {
            get => _currentOperation;
            protected set
            {
                _currentOperation?.Dispose();
                _currentOperation = value;
            }
        }

        public IAnalyticsSession CurrentSession
        {
            get => /*_currentSession ??=*/ _sessionFactory.CreateSession();
            protected set => _currentSession = value;
        } 

        public IDictionary<string, string> AnalyticsOperationHeaders
        {
            get
            {
                var headers = new Dictionary<string, string>
                {
                    [AnalyticsHeaders.Operation.Name] = CurrentOperation?.Name ?? string.Empty,
                    [AnalyticsHeaders.Operation.Id] = CurrentOperation?.Id ?? string.Empty
                };

                if (_currentSession != null)
                {
                    headers[AnalyticsHeaders.Session.Id] = CurrentSession.Id;
                    
                    if(CurrentSession.AccountId != null)
                        headers[AnalyticsHeaders.Session.AccountId] = CurrentSession.AccountId;
                    
                    if(CurrentSession.UserId != null)
                        headers[AnalyticsHeaders.Session.UserId] = CurrentSession.UserId;
                    
                    if(CurrentSession.DeviceId != null)
                        headers[AnalyticsHeaders.Session.DeviceId] = CurrentSession.DeviceId;
                    
                    if(CurrentSession.AppVersion != null)
                        headers[AnalyticsHeaders.Session.AppVersion] = CurrentSession.AppVersion;
                
                    foreach (var currentSessionProperty in CurrentSession.Properties)
                    {
                        headers[AnalyticsHeaders.Prefix + currentSessionProperty.Key] = currentSessionProperty.Value;
                    }
                }
                
                return headers;
            }
        }

        public void ResetCurrentSession(string newSessionId = "")
        {
            CurrentOperation?.Dispose();
            CurrentOperation = null;
            var sessionId = string.IsNullOrEmpty(newSessionId) ? CurrentSession.Id : newSessionId;
            CurrentSession = AnalyticsSession.FromExisting(sessionId);
        }

         
        public virtual IAnalyticsOperation StartRequestOperation(object sender, string requestName, IDictionary<string, string> headers, string callingMember = "")
        {
            if (!headers.TryGetValue(AnalyticsHeaders.Operation.Name, out var operationName))
                operationName = "NewRequest";

            if (!headers.TryGetValue(AnalyticsHeaders.Operation.Id, out var operationId))
                operationId = Guid.NewGuid().ToString();

            if (!headers.TryGetValue(AnalyticsHeaders.Session.Id, out var sessionId))
                sessionId = Guid.NewGuid().ToString();
            
            var session = AnalyticsSession.FromExisting(sessionId);

            if (headers.TryGetValue(AnalyticsHeaders.Session.AppVersion, out var appVersion))
                session.AppVersion = appVersion;

            if (headers.TryGetValue(AnalyticsHeaders.Session.AccountId, out var accountId))
                session.AccountId = accountId;

            if (headers.TryGetValue(AnalyticsHeaders.Session.DeviceId, out var deviceId))
                session.DeviceId = deviceId;
            
            if (headers.TryGetValue(AnalyticsHeaders.Session.UserId, out var userId))
                session.UserId = userId;

            foreach (var header in headers)
            {
                if (header.Key.StartsWith(AnalyticsHeaders.Prefix))
                {
                    var propertyName = header.Key.Substring(AnalyticsHeaders.Prefix.Length);
                    session.SetProperty(propertyName, header.Value);
                }
            }

            CurrentSession = session;

            HandleNewRequest(session, operationName, operationId);

            CurrentOperation = new AnalyticsOperation(operationName, operationId, duration =>
            {
                GetLogger(sender, new Dictionary<string, object>())
                    .Verbose("Request {OperationName} handled in {Duration}", operationName, duration);
                _currentOperation = null;
            });

            return CurrentOperation;
        }
         

        public IAnalyticsOperation StartPageViewOperation(object sender, string pageName = "", Dictionary<string, object> properties = null, string callingMember = "")
        {
            throw new NotImplementedException();
        }

        public IAnalyticsOperation StartOperation(object sender, string operationName, Dictionary<string, object> properties = null, string callingMember = "")
        {
            _analyticsContext.Set(AnalyticsHeaders.Operation.Name, operationName);
            _analyticsContext.Set(AnalyticsHeaders.Operation.Id, Guid.NewGuid());
            
            if (string.IsNullOrWhiteSpace(operationName))
            {
                operationName = sender.GetType().Name;
            }  

            CurrentOperation = new AnalyticsOperation(operationName, duration =>
            { 
                properties ??= new Dictionary<string, object>();
                GetLogger(sender, properties)
                    .Verbose("Operation {OperationName} completed in {Duration}", operationName, duration);
                _currentOperation = null;
            });

            return CurrentOperation;
        }

        public IAnalyticsOperation StartTrace(object sender, string trace, LogSeverity logSeverity = LogSeverity.Verbose, Dictionary<string, object> properties = null, string callerMemberName = "")
        {
            properties ??= new Dictionary<string, object>();

            return new AnalyticsOperation(trace, duration =>
            {
                GetLogger(sender, properties)
                    .Write(logSeverity.ToLogEventLevel(), "Trace {Trace} completed in {Duration}", trace, duration);
            });
        }
         
        public void Trace(object sender, string message, LogSeverity logSeverity = LogSeverity.Verbose, Dictionary<string, object> properties = null, string callingMember = "")
        {
            
            GetLogger(sender, properties)
                .Write(logSeverity.ToLogEventLevel(),message);
        }

        public void LogEvent(object sender, string eventName, Dictionary<string, object> properties = null, string callingMember = "")
        {
            GetLogger(sender, properties)
                .Information(eventName);
        }

        public void LogException(object sender, Exception exception, Dictionary<string, object> properties = null, string callingMember = "")
        {
            GetLogger(sender, properties)
                .Error(exception, "Error occured {Exception}", exception.Message);
        }

        public void Debug(string message, Dictionary<string, object> properties = null)
        {
            Log.Debug(message);
        }

        public void Debug(Func<string> messageFactory)
        {
            Log.Debug(messageFactory.Invoke());
        }

        
        protected ILogger GetLogger(object sender, Dictionary<string, object>? properties = null)
        {
            var logger = Log.ForContext(sender.GetType());

            if (CurrentOperation != null)
            {
                logger = logger.ForContext("OperationId", CurrentOperation.Id);
                logger = logger.ForContext("OperationName", CurrentOperation.Name);
            }

            var currentSession = CurrentSession;
            if (currentSession != null)
            {
                if(!string.IsNullOrEmpty(currentSession.Id))
                    logger = logger.ForContext("SessionId", currentSession.Id);

                if(!string.IsNullOrEmpty(currentSession.UserId))
                    logger = logger.ForContext("UserId", currentSession.UserId);
                
                if(!string.IsNullOrEmpty(currentSession.DeviceId))
                    logger = logger.ForContext("DeviceId", currentSession.DeviceId);
                
                if(!string.IsNullOrEmpty(currentSession.AccountId))
                    logger = logger.ForContext("AccountId", currentSession.AccountId);

                if(!string.IsNullOrEmpty(currentSession.AppVersion))
                    logger = logger.ForContext("UserId", currentSession.AppVersion);

                if(currentSession.DeviceType!=null && !string.IsNullOrEmpty(currentSession.DeviceType.Value))
                    logger = logger.ForContext("UserId", currentSession.DeviceType.Value);

                if(currentSession.Platform!=null && !string.IsNullOrEmpty(currentSession.Platform.Value))
                    logger = logger.ForContext("UserId", currentSession.Platform.Value);

                if(!string.IsNullOrEmpty(currentSession.OperatingSystemVersion))
                    logger = logger.ForContext("UserId", currentSession.OperatingSystemVersion);
            }

            if (properties != null)
            {
                foreach (var property in properties)
                {
                    logger = logger.ForContext(property.Key, property.Value);
                }
            }

            return logger;
        }

        protected void HandleNewRequest(AnalyticsSession session, string operationName, string operationId)
        {
            _analyticsContext.Set(AnalyticsHeaders.Operation.Name, operationName);
            _analyticsContext.Set(AnalyticsHeaders.Operation.Id, operationId);
            
            _analyticsContext.Set(AnalyticsHeaders.Session.Id, session.Id);
            _analyticsContext.Set(AnalyticsHeaders.Session.AccountId, session.AccountId ?? string.Empty);
            _analyticsContext.Set(AnalyticsHeaders.Session.UserId, session.UserId ?? string.Empty);
            _analyticsContext.Set(AnalyticsHeaders.Session.DeviceId, session.DeviceId ?? string.Empty);
            _analyticsContext.Set(AnalyticsHeaders.Session.AppVersion, session.AppVersion ?? string.Empty);
        }
    }
}