using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Blauhaus.Analytics.Abstractions.Http;
using Blauhaus.Analytics.Abstractions.Operation;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Analytics.Abstractions.Session;
using Blauhaus.Common.ValueObjects.BuildConfigs;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.Extensions.Logging;

namespace Blauhaus.Analytics.ILogger
{
    public class LoggerAnalyticsService : IAnalyticsService
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly IBuildConfig _currentBuildConfig;
        private readonly IAnalyticsSessionFactory _sessionFactory;

        private Microsoft.Extensions.Logging.ILogger GetLogger(object source) => _loggerFactory.CreateLogger(source.GetType().Name);

        public LoggerAnalyticsService(
            ILoggerFactory loggerFactory,
            IBuildConfig currentBuildConfig,
            IAnalyticsSessionFactory sessionFactory)
        {
            _loggerFactory = loggerFactory;
            _currentBuildConfig = currentBuildConfig;
            _sessionFactory = sessionFactory;
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

        public virtual IAnalyticsSession CurrentSession
        {
            get => _currentSession ??= _sessionFactory.CreateSession();
            protected set
            {
                _currentSession = value;
            }
        } 

        private readonly Dictionary<string, string> _analyticsOperationHeaders = new();
        private IAnalyticsOperation _currentOperation;
        private IAnalyticsSession _currentSession;

        public IDictionary<string, string> AnalyticsOperationHeaders
        {
            get
            {
                _analyticsOperationHeaders[AnalyticsHeaders.Operation.Name] = CurrentOperation?.Name ?? string.Empty;
                _analyticsOperationHeaders[AnalyticsHeaders.Operation.Id] = CurrentOperation?.Id ?? string.Empty;

                if (CurrentSession != null)
                {
                    _analyticsOperationHeaders[AnalyticsHeaders.Session.Id] = CurrentSession.Id;
                    
                    if(CurrentSession.AccountId != null)
                        _analyticsOperationHeaders[AnalyticsHeaders.Session.AccountId] = CurrentSession.AccountId;
                    
                    if(CurrentSession.UserId != null)
                        _analyticsOperationHeaders[AnalyticsHeaders.Session.UserId] = CurrentSession.UserId;
                    
                    if(CurrentSession.DeviceId != null)
                        _analyticsOperationHeaders[AnalyticsHeaders.Session.DeviceId] = CurrentSession.DeviceId;
                    
                    if(CurrentSession.AppVersion != null)
                        _analyticsOperationHeaders[AnalyticsHeaders.Session.AppVersion] = CurrentSession.AppVersion;
                
                    foreach (var currentSessionProperty in CurrentSession.Properties)
                    {
                        _analyticsOperationHeaders[AnalyticsHeaders.Prefix + currentSessionProperty.Key] = currentSessionProperty.Value;
                    }
                }
                
                return _analyticsOperationHeaders;
            }
        }
        
        public void ResetCurrentSession(string newSessionId = "")
        {
            CurrentOperation?.Dispose();
            CurrentOperation = null;
            var sessionId = string.IsNullOrEmpty(newSessionId) ? CurrentSession.Id : newSessionId;
            CurrentSession = AnalyticsSession.FromExisting(sessionId);
        }

        
        public IAnalyticsOperation StartRequestOperation(object sender, string requestName, IDictionary<string, string> headers, [CallerMemberName] string callingMember = "")
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
                var requestTelemetry = new RequestTelemetry
                {
                    Duration = duration,
                    Name = requestName
                };
                
                GetLogger(sender).LogInformation(CurrentOperation?.Name);

                _currentOperation = null;
            });

            return CurrentOperation;
        }

        protected virtual void HandleNewRequest(AnalyticsSession session, string operationName, string operationId)
        {
        }

        public IAnalyticsOperation StartPageViewOperation(object sender, string viewName = "", Dictionary<string, object> properties = null, string callingMember = "")
        {
            throw new NotImplementedException();
        }

        public IAnalyticsOperation StartOperation(object sender, string operationName, Dictionary<string, object> properties = null, string callingMember = "")
        {
            var callingClassName = sender.GetType().Name;
            

            CurrentOperation = new AnalyticsOperation(operationName, duration =>
            {
                var dependencyTelemetry = new DependencyTelemetry
                {
                    Duration = duration,
                    Name = operationName,
                };

                GetLogger(sender).LogTrace("Operation {op} completed in {dur}", operationName, duration);

                _currentOperation = null;
            });

            return CurrentOperation;
        }

        public IAnalyticsOperation StartTrace(object sender, string message, LogSeverity logSeverity = LogSeverity.Verbose, Dictionary<string, object> properties = null, string callerMemberName = "")
        {
            return new AnalyticsOperation(message, duration =>
            {
                properties["Duration"] = duration;
                GetLogger(sender).LogTrace("{op} completed in {dur}", message, duration);
            });
        }

        public void Trace(object sender, string message, LogSeverity logSeverityLevel = LogSeverity.Verbose, Dictionary<string, object> properties = null, string callingMember = "")
        {
            GetLogger(sender).Log(Convert(logSeverityLevel), message);
        }

        public void LogEvent(object sender, string eventName, Dictionary<string, object> properties = null, string callingMember = "")
        {
            GetLogger(sender).Log(LogLevel.Information, eventName);
        }

        public void LogException(object sender, Exception exception, Dictionary<string, object> properties = null, string callingMember = "")
        {
            GetLogger(sender).Log(LogLevel.Error, exception, callingMember);
        }

        public void Debug(string message, Dictionary<string, object> properties = null)
        {
            if (_currentBuildConfig.Equals(BuildConfig.Debug))
            {
                System.Console.Out.WriteLine(message);
            }
        }

        public void Debug(Func<string> messageFactory)
        {
            if (_currentBuildConfig.Equals(BuildConfig.Debug))
            {
                System.Console.Out.WriteLine(messageFactory.Invoke());
            }
        }

        private static LogLevel Convert(LogSeverity severity)
        {
            switch (severity)
            {
                case LogSeverity.Critical:
                    return LogLevel.Critical;
                case LogSeverity.Error:
                    return LogLevel.Error;
                case LogSeverity.Verbose:
                    return LogLevel.Trace;
                case LogSeverity.Information:
                    return LogLevel.Information;
                case LogSeverity.Warning:
                    return LogLevel.Warning;
                case LogSeverity.Debug:
                    return LogLevel.Debug;
                default:
                    throw new ArgumentOutOfRangeException(nameof(severity), severity, null);
            }
        }
    }
}