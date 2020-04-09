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
        public static IAnalyticsService TraceVerbose(this IAnalyticsService analyticsService, object sender, string message, object property, [CallerMemberName] string caller = "")
        {
            if (property is Dictionary<string, object> objectProperties)
            {
                analyticsService.Trace(sender, message, LogSeverity.Verbose, objectProperties, caller);
            }
            else
            {
                analyticsService.Trace(sender, message, LogSeverity.Verbose, property.ToObjectDictionary(), caller);
            }
            return analyticsService;
        }
        public static IAnalyticsService TraceVerbose(this IAnalyticsService analyticsService, object sender, string message, string propertyName, string propertyValue, [CallerMemberName] string caller = "")
        {
            analyticsService.Trace(sender, message, LogSeverity.Verbose, new Dictionary<string, object>{{propertyName, propertyValue}}, caller);
            return analyticsService;
        }

        public static IAnalyticsService TraceInformation(this IAnalyticsService analyticsService, object sender, string message, [CallerMemberName] string caller = "")
        {
            analyticsService.Trace(sender, message, LogSeverity.Information, new Dictionary<string, object>(), caller);
            return analyticsService;
        }
        public static IAnalyticsService TraceInformation(this IAnalyticsService analyticsService, object sender, string message, object property, [CallerMemberName] string caller = "")
        {
            if (property is Dictionary<string, object> objectProperties)
            {
                analyticsService.Trace(sender, message, LogSeverity.Information, objectProperties, caller);
            }
            else
            {
                analyticsService.Trace(sender, message, LogSeverity.Information, property.ToObjectDictionary(), caller);
            }
            return analyticsService;
        }
        public static IAnalyticsService TraceInformation(this IAnalyticsService analyticsService, object sender, string message, string propertyName, string propertyValue, [CallerMemberName] string caller = "")
        {
            analyticsService.Trace(sender, message, LogSeverity.Information, new Dictionary<string, object>{{propertyName, propertyValue}}, caller);
            return analyticsService;
        }

        
        public static IAnalyticsService TraceWarning(this IAnalyticsService analyticsService, object sender, string message, [CallerMemberName] string caller = "")
        {
            analyticsService.Trace(sender, message, LogSeverity.Warning, new Dictionary<string, object>(), caller);
            return analyticsService;
        }
        public static IAnalyticsService TraceWarning(this IAnalyticsService analyticsService, object sender, string message, object property, [CallerMemberName] string caller = "")
        {
            if (property is Dictionary<string, object> objectProperties)
            {
                analyticsService.Trace(sender, message, LogSeverity.Warning, objectProperties, caller);
            }
            else
            {
                analyticsService.Trace(sender, message, LogSeverity.Warning, property.ToObjectDictionary(), caller);
            }
            return analyticsService;
        }
        public static IAnalyticsService TraceWarning(this IAnalyticsService analyticsService, object sender, string message, string propertyName, string propertyValue, [CallerMemberName] string caller = "")
        {
            analyticsService.Trace(sender, message, LogSeverity.Warning, new Dictionary<string, object>{{propertyName, propertyValue}}, caller);
            return analyticsService;
        }
        
        
        public static IAnalyticsService TraceCritical(this IAnalyticsService analyticsService, object sender, string message, [CallerMemberName] string caller = "")
        {
            analyticsService.Trace(sender, message, LogSeverity.Critical, new Dictionary<string, object>(), caller);
            return analyticsService;
        }
        public static IAnalyticsService TraceCritical(this IAnalyticsService analyticsService, object sender, string message, object property, [CallerMemberName] string caller = "")
        {
            if (property is Dictionary<string, object> objectProperties)
            {
                analyticsService.Trace(sender, message, LogSeverity.Critical, objectProperties, caller);
            }
            else
            {
                analyticsService.Trace(sender, message, LogSeverity.Critical, property.ToObjectDictionary(), caller);
            }
            return analyticsService;
        }
        public static IAnalyticsService TraceCritical(this IAnalyticsService analyticsService, object sender, string message, string propertyName, string propertyValue, [CallerMemberName] string caller = "")
        {
            analyticsService.Trace(sender, message, LogSeverity.Critical, new Dictionary<string, object>{{propertyName, propertyValue}}, caller);
            return analyticsService;
        }

        
        public static IAnalyticsService TraceError(this IAnalyticsService analyticsService, object sender, string message, [CallerMemberName] string caller = "")
        {
            analyticsService.Trace(sender, message, LogSeverity.Error, new Dictionary<string, object>(), caller);
            return analyticsService;
        }
        public static IAnalyticsService TraceError(this IAnalyticsService analyticsService, object sender, string message, object property, [CallerMemberName] string caller = "")
        {
            if (property is Dictionary<string, object> objectProperties)
            {
                analyticsService.Trace(sender, message, LogSeverity.Error, objectProperties, caller);
            }
            else
            {
                analyticsService.Trace(sender, message, LogSeverity.Error, property.ToObjectDictionary(), caller);
            }
            return analyticsService;
        }
        public static IAnalyticsService TraceError(this IAnalyticsService analyticsService, object sender, string message, string propertyName, string propertyValue, [CallerMemberName] string caller = "")
        {
            analyticsService.Trace(sender, message, LogSeverity.Error, new Dictionary<string, object>{{propertyName, propertyValue}}, caller);
            return analyticsService;
        }
        public static string TraceError(this IAnalyticsService analyticsService, object sender, Error error, [CallerMemberName] string caller = "")
        {
            analyticsService.Trace(sender, error.Code, LogSeverity.Error, new Dictionary<string, object>(), caller);
            return error.ToString();
        }
        public static string TraceError(this IAnalyticsService analyticsService, object sender, Error error, object property, [CallerMemberName] string caller = "")
        {
            analyticsService.Trace(sender, error.Code, LogSeverity.Error, property.ToObjectDictionary(), caller);
            return error.ToString();
        }
        public static string TraceError(this IAnalyticsService analyticsService, object sender, Error error, string propertyName, string propertyValue, [CallerMemberName] string caller = "")
        {
            analyticsService.Trace(sender, error.Code, LogSeverity.Error, new Dictionary<string, object>{{propertyName, propertyValue}}, caller);
            return error.ToString();
        }
    }
}