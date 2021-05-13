using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Blauhaus.Analytics.Abstractions.Config;
using Blauhaus.Analytics.Abstractions.Extensions;
using Blauhaus.Analytics.Abstractions.Http;
using Blauhaus.Analytics.Abstractions.Operation;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Analytics.Abstractions.Session;
using Blauhaus.Analytics.Common.Extensions;
using Blauhaus.Analytics.Common.Telemetry;
using Blauhaus.Analytics.Console.ConsoleLoggers;
using Blauhaus.Common.ValueObjects.BuildConfigs;
using Microsoft.ApplicationInsights.DataContracts;

namespace Blauhaus.Analytics.Common.Service
{
    public class AnalyticsService : IAnalyticsService
    {
        private IAnalyticsSession _currentSession;
        private IAnalyticsOperation? _currentOperation;
        private static readonly Dictionary<string, object> EmptyProperties = new Dictionary<string, object>();

        protected readonly IApplicationInsightsConfig Config;
        protected readonly IConsoleLogger ConsoleLogger;
        protected readonly IBuildConfig CurrentBuildConfig;
        private readonly IAnalyticsSessionFactory _sessionFactory;
        protected readonly ITelemetryClientProxy TelemetryClient;
        protected readonly ITelemetryDecorator TelemetryDecorator;

        public AnalyticsService(
            IApplicationInsightsConfig config, 
            IConsoleLogger consoleLogger, 
            ITelemetryClientProxy telemetryClient, 
            ITelemetryDecorator telemetryDecorator,
            IBuildConfig currentBuildConfig,
            IAnalyticsSessionFactory sessionFactory)
        {
            Config = config;
            ConsoleLogger = consoleLogger;
            TelemetryClient = telemetryClient;
            TelemetryDecorator = telemetryDecorator;
            CurrentBuildConfig = currentBuildConfig;
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
            get
            {
                if (_currentSession == null)
                {
                    _currentSession = _sessionFactory.CreateSession();
                }

                return _currentSession;
            }
            protected set
            {
                _currentSession = value;
            }
        } 

        private readonly Dictionary<string, string> _analyticsOperationHeaders = new Dictionary<string, string>();
        public IDictionary<string, string> AnalyticsOperationHeaders
        {
            get
            {
                _analyticsOperationHeaders[AnalyticsHeaders.Operation.Name] = CurrentOperation?.Name;
                _analyticsOperationHeaders[AnalyticsHeaders.Operation.Id] = CurrentOperation?.Id;

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
                
                TelemetryClient.TrackRequest(TelemetryDecorator.DecorateTelemetry(requestTelemetry, sender.GetType().Name, callingMember, CurrentOperation, CurrentSession, new Dictionary<string, object>()));

                ConsoleLogger.LogOperation(requestName, duration);

                _currentOperation = null;
            });

            return CurrentOperation;
        }

        protected virtual void HandleNewRequest(AnalyticsSession session, string operationName, string operationId)
        {
        }

        public IAnalyticsOperation StartPageViewOperation(object sender, string pageName = "", Dictionary<string, object>? properties = null, [CallerMemberName] string callerMember = "")
        {
            properties ??= EmptyProperties;

            if (string.IsNullOrWhiteSpace(pageName))
            {
                pageName = sender.GetType().Name;
            }  

            CurrentOperation = new AnalyticsOperation(pageName, duration =>
            {
                var pageViewTelemetry = new PageViewTelemetry(pageName)
                {
                    Duration = duration
                };
                
                properties ??= new Dictionary<string, object>();

                TelemetryClient.TrackPageView(TelemetryDecorator.DecorateTelemetry(pageViewTelemetry, sender.GetType().Name, callerMember, CurrentOperation, CurrentSession, properties));

                ConsoleLogger.LogOperation(pageName, duration);
                
                properties["Duration"] = duration;
                HandleOperationCompleted(sender, properties, callerMember, pageName);

                _currentOperation = null;
            });

            return CurrentOperation;
        }

        public IAnalyticsOperation StartOperation(object sender, string operationName, Dictionary<string, object>? properties = null, [CallerMemberName] string callerMemberName = "")
        {
            var callingClassName = sender.GetType().Name;
            
            if (properties == null) properties = EmptyProperties;

            CurrentOperation = new AnalyticsOperation(operationName, duration =>
            {
                var dependencyTelemetry = new DependencyTelemetry
                {
                    Duration = duration,
                    Name = operationName,
                };
                
                if(properties == null) properties = new Dictionary<string, object>();

                TelemetryDecorator.DecorateTelemetry(dependencyTelemetry, callingClassName, callerMemberName, CurrentOperation, CurrentSession, properties);
                TelemetryClient.TrackDependency(dependencyTelemetry);
                ConsoleLogger.LogOperation(operationName, duration);

                properties["Duration"] = duration;
                HandleOperationCompleted(sender, properties, callerMemberName, operationName);

                _currentOperation = null;
            });

            return CurrentOperation;
        }

        protected virtual void HandleOperationCompleted(object sender, Dictionary<string, object>? properties, string callerMemberName, string operationName)
        {
        }

        public virtual void LogEvent(object sender, string eventName, Dictionary<string, object>? properties = null, [CallerMemberName] string callerMemberName = "")
        {
            if (properties == null) properties = EmptyProperties;

            TelemetryClient.TrackEvent(TelemetryDecorator
                .DecorateTelemetry(new EventTelemetry(eventName), sender.GetType().Name, callerMemberName, CurrentOperation, CurrentSession, properties));

            ConsoleLogger.LogEvent(eventName, properties.ToDictionaryOfStrings());
        }

        public virtual void LogException(object sender, Exception exception, Dictionary<string, object>? properties = null, [CallerMemberName] string callerMemberName = "")
        {
            properties ??= EmptyProperties;
            properties["ExceptionType"] = exception.GetType().FullName;

            TelemetryClient.TrackException(TelemetryDecorator
                .DecorateTelemetry(new ExceptionTelemetry(exception), sender.GetType().Name, callerMemberName, CurrentOperation, CurrentSession, properties));
            
            ConsoleLogger.LogException(exception, properties.ToDictionaryOfStrings());
        }

        public void Debug(string message, Dictionary<string, object>? properties = null)
        {
            if (CurrentBuildConfig.Equals(BuildConfig.Debug))
            {
                ConsoleLogger.LogTrace(message, LogSeverity.Debug, properties.ToDictionaryOfStrings());
            }
        }

        public void Trace(object sender, string message, LogSeverity logSeverity = LogSeverity.Verbose, Dictionary<string, object>? properties = null, [CallerMemberName] string callerMemberName = "")
        {
            
            if (properties == null) properties = EmptyProperties;

            LogTrace(message, logSeverity, properties, sender.GetType().Name, callerMemberName);
        }

        public IAnalyticsOperation StartTrace(object sender, string message, LogSeverity logSeverity = LogSeverity.Verbose, Dictionary<string, object>? properties = null, [CallerMemberName] string callerMemberName = "")
        {
            if (properties == null) properties = new Dictionary<string, object>();

            return new AnalyticsOperation(message, duration =>
            {
                properties["Duration"] = duration;
                LogTrace(message, logSeverity, properties, sender.GetType().Name, callerMemberName);
            });
        }

        protected virtual void LogTrace(string message, LogSeverity logSeverity, Dictionary<string, object> properties, string callingClassName, string callerMemberName)
        {
            if (Config.MinimumLogToServerSeverity.TryGetValue(CurrentBuildConfig, out var minumumSeverityToLogToServer))
            {
                if (logSeverity >= minumumSeverityToLogToServer)
                {
                    TelemetryClient.TrackTrace(TelemetryDecorator
                        .DecorateTelemetry(new TraceTelemetry(message, logSeverity.ToSeverityLevel()), callingClassName, callerMemberName, CurrentOperation, CurrentSession, properties));
                }
            }

            ConsoleLogger.LogTrace(message, logSeverity, properties.ToDictionaryOfStrings());
        }   



    }

    
}