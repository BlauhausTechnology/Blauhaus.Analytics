using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Common.ValueObjects.Errors;

namespace Blauhaus.Analytics.Abstractions.Extensions
{
    public static class AnalyticsServiceExtensions
    {
        public static IAnalyticsService TraceVerbose(this IAnalyticsService analyticsService, object sender, string message, [CallerMemberName] string caller = "")
        {
            analyticsService.Trace(sender, message, LogSeverity.Verbose, new Dictionary<string, object>(), caller);
            return analyticsService;
        }
        public static IAnalyticsService TraceVerbose(this IAnalyticsService analyticsService, object sender, string message, string propertyName, string propertyValue, [CallerMemberName] string caller = "")
        {
            analyticsService.Trace(sender, message, LogSeverity.Verbose, new Dictionary<string, object> {{propertyName, propertyValue}}, caller);
            return analyticsService;
        }
        public static IAnalyticsService TraceVerbose(this IAnalyticsService analyticsService, object sender, string message, Dictionary<string, object> properties, [CallerMemberName] string caller = "")
        {
            analyticsService.Trace(sender, message, LogSeverity.Verbose, properties, caller);
            return analyticsService;
        }

        public static IAnalyticsService TraceInformation(this IAnalyticsService analyticsService, object sender, string message, [CallerMemberName] string caller = "")
        {
            analyticsService.Trace(sender, message, LogSeverity.Information, new Dictionary<string, object>(), caller);
            return analyticsService;
        } 
        public static IAnalyticsService TraceInformation(this IAnalyticsService analyticsService, object sender, string message, string propertyName, string propertyValue, [CallerMemberName] string caller = "")
        {
            analyticsService.Trace(sender, message, LogSeverity.Information, new Dictionary<string, object> {{propertyName, propertyValue}}, caller);
            return analyticsService;
        }
        public static IAnalyticsService TraceInformation(this IAnalyticsService analyticsService, object sender, string message, Dictionary<string, object> properties, [CallerMemberName] string caller = "")
        {
            analyticsService.Trace(sender, message, LogSeverity.Information, properties, caller);
            return analyticsService;
        }


        public static IAnalyticsService TraceWarning(this IAnalyticsService analyticsService, object sender, string message, [CallerMemberName] string caller = "")
        {
            analyticsService.Trace(sender, message, LogSeverity.Warning, new Dictionary<string, object>(), caller);
            return analyticsService;
        }
        public static IAnalyticsService TraceWarning(this IAnalyticsService analyticsService, object sender, string message, string propertyName, string propertyValue, [CallerMemberName] string caller = "")
        {
            analyticsService.Trace(sender, message, LogSeverity.Warning, new Dictionary<string, object> {{propertyName, propertyValue}}, caller);
            return analyticsService;
        }
        public static IAnalyticsService TraceWarning(this IAnalyticsService analyticsService, object sender, string message, Dictionary<string, object> properties, [CallerMemberName] string caller = "")
        {
            analyticsService.Trace(sender, message, LogSeverity.Warning, properties, caller);
            return analyticsService;
        }


        public static IAnalyticsService TraceCritical(this IAnalyticsService analyticsService, object sender, string message, [CallerMemberName] string caller = "")
        {
            analyticsService.Trace(sender, message, LogSeverity.Critical, new Dictionary<string, object>(), caller);
            return analyticsService;
        }
        public static IAnalyticsService TraceCritical(this IAnalyticsService analyticsService, object sender, string message, string propertyName, string propertyValue, [CallerMemberName] string caller = "")
        {
            analyticsService.Trace(sender, message, LogSeverity.Critical, new Dictionary<string, object> {{propertyName, propertyValue}}, caller);
            return analyticsService;
        }
        public static IAnalyticsService TraceCritical(this IAnalyticsService analyticsService, object sender, string message, Dictionary<string, object> properties, [CallerMemberName] string caller = "")
        {
            analyticsService.Trace(sender, message, LogSeverity.Critical, properties, caller);
            return analyticsService;
        }


        public static IAnalyticsService TraceError(this IAnalyticsService analyticsService, object sender, string message, [CallerMemberName] string caller = "")
        {
            analyticsService.Trace(sender, message, LogSeverity.Error, new Dictionary<string, object>(), caller);
            return analyticsService;
        }
        public static IAnalyticsService TraceError(this IAnalyticsService analyticsService, object sender, string message, string propertyName, string propertyValue, [CallerMemberName] string caller = "")
        {
            analyticsService.Trace(sender, message, LogSeverity.Error, new Dictionary<string, object> {{propertyName, propertyValue}}, caller);
            return analyticsService;
        }
        public static IAnalyticsService TraceError(this IAnalyticsService analyticsService, object sender, string message, Dictionary<string, object> properties, [CallerMemberName] string caller = "")
        {
            analyticsService.Trace(sender, message, LogSeverity.Error, properties, caller);
            return analyticsService;
        }

        //Error extensions
        public static string TraceError(this IAnalyticsService analyticsService, object sender, Error error,
            LogSeverity logSeverity = LogSeverity.Error, [CallerMemberName] string caller = "")
        {
            analyticsService.Trace(sender, error.Code, logSeverity, new Dictionary<string, object>(), caller);
            return error.ToString();
        } 
        public static string TraceError(this IAnalyticsService analyticsService, object sender, Error error, string propertyName, string propertyValue,
            LogSeverity logSeverity = LogSeverity.Error, [CallerMemberName] string caller = "")
        {
            analyticsService.Trace(sender, error.Code, logSeverity, new Dictionary<string, object> {{propertyName, propertyValue}}, caller);
            return error.ToString();
        }
        public static string TraceError(this IAnalyticsService analyticsService, object sender, Error error, Dictionary<string, object> properties,
            LogSeverity logSeverity = LogSeverity.Error, [CallerMemberName] string caller = "")
        {
            analyticsService.Trace(sender, error.Code, logSeverity, properties, caller);
            return error.ToString();
        }

    
    }
}