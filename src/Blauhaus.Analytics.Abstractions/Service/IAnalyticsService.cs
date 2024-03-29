﻿using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Blauhaus.Analytics.Abstractions.Operation;
using Blauhaus.Analytics.Abstractions.Session;

namespace Blauhaus.Analytics.Abstractions.Service
{
    public interface IAnalyticsService
    {
        IAnalyticsOperation? CurrentOperation { get; }
        IAnalyticsSession CurrentSession { get; }
        IDictionary<string, string> AnalyticsOperationHeaders { get; }
        void ResetCurrentSession(string newSessionId = "");

        IAnalyticsOperation StartRequestOperation(object sender, string requestName, IDictionary<string, string> headers, [CallerMemberName] string callingMember = "");
        IAnalyticsOperation StartPageViewOperation(object sender, string viewName = "", Dictionary<string, object>? properties = null,  [CallerMemberName] string callingMember = "");
        IAnalyticsOperation StartOperation(object sender, string operationName, Dictionary<string, object>? properties = null, [CallerMemberName] string callingMember = "");

        IAnalyticsOperation StartTrace(object sender, string message, LogSeverity logSeverity = LogSeverity.Verbose, Dictionary<string, object>? properties = null, [CallerMemberName] string callerMemberName = "");

        void Trace(object sender, string message, LogSeverity logSeverityLevel = 0, Dictionary<string, object>? properties = null, [CallerMemberName] string callingMember = "");
        void LogEvent(object sender, string eventName, Dictionary<string, object>? properties = null, [CallerMemberName] string callingMember = "");
        void LogException(object sender, Exception exception, Dictionary<string, object>? properties = null, [CallerMemberName] string callingMember = "");
        
        [Obsolete("Use the messageFactory instead")]
        void Debug(string message, Dictionary<string, object>? properties = null);
        void Debug(Func<string> messageFactory);
    }
}