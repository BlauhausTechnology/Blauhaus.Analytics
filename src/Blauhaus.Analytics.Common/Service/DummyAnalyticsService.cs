using System;
using System.Collections.Generic;
using Blauhaus.Analytics.Abstractions.Operation;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Analytics.Abstractions.Session;

namespace Blauhaus.Analytics.Common.Service
{

    //this is just so we have a placeholder while switching from IAnalyticsService to IAnalyticsLogger<T>
    public class DummyAnalyticsService : IAnalyticsService
    {
        public void ResetCurrentSession(string newSessionId = "")
        {

        }

        public IAnalyticsOperation StartRequestOperation(object sender, string requestName, IDictionary<string, string> headers, string callingMember = "")
        {
            return CurrentOperation!;
        }

        public IAnalyticsOperation StartPageViewOperation(object sender, string viewName = "", Dictionary<string, object>? properties = null, string callingMember = "")
        {
            return CurrentOperation!;
        }

        public IAnalyticsOperation StartOperation(object sender, string operationName, Dictionary<string, object>? properties = null, string callingMember = "")
        {
            return CurrentOperation!;
        }

        public IAnalyticsOperation StartTrace(object sender, string message, LogSeverity logSeverity = LogSeverity.Verbose, Dictionary<string, object>? properties = null, string callerMemberName = "")
        {
            return CurrentOperation!;
        }

        public void Trace(object sender, string message, LogSeverity logSeverityLevel = LogSeverity.Verbose, Dictionary<string, object>? properties = null, string callingMember = "")
        {
        }

        public void LogEvent(object sender, string eventName, Dictionary<string, object>? properties = null, string callingMember = "")
        {
        }

        public void LogException(object sender, Exception exception, Dictionary<string, object>? properties = null, string callingMember = "")
        {
        }

        public void Debug(string message, Dictionary<string, object>? properties = null)
        {
        }

        public void Debug(Func<string> messageFactory)
        {
        }

        public IAnalyticsOperation? CurrentOperation { get; } = new AnalyticsOperation("none", t => { });
        public IAnalyticsSession CurrentSession { get; } = AnalyticsSession.Empty;
        public IDictionary<string, string> AnalyticsOperationHeaders { get; } = new Dictionary<string, string>();
    }
}