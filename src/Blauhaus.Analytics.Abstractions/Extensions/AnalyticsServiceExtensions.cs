using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Errors;
using Blauhaus.Responses;

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
            analyticsService.Trace(sender, error.ToString(), logSeverity, new Dictionary<string, object>
            {
                {"ErrorDescription", error.Description },
                {"ErrorCode", error.Code}
            }, caller);
            return error.ToString();
        } 
        public static string TraceError(this IAnalyticsService analyticsService, object sender, Error error, string propertyName, string propertyValue,
            LogSeverity logSeverity = LogSeverity.Error, [CallerMemberName] string caller = "")
        {
            analyticsService.Trace(sender, error.ToString(), logSeverity, new Dictionary<string, object>
            {
                {"ErrorDescription", error.Description },
                {"ErrorCode", error.Code},
                {propertyName, propertyValue}
            }, caller);
            return error.ToString();
        }
        public static string TraceError(this IAnalyticsService analyticsService, object sender, Error error, Dictionary<string, object> properties,
            LogSeverity logSeverity = LogSeverity.Error, [CallerMemberName] string caller = "")
        {
            properties["ErrorDescription"] = error.Description;
            properties["ErrorCode"] = error.Code;
            analyticsService.Trace(sender, error.ToString(), logSeverity, properties, caller);
            return error.ToString();
        }

        public static string LogExceptionError(this IAnalyticsService analyticsService, object sender, 
            Exception e, Error error, Dictionary<string, object> properties = null, [CallerMemberName] string caller = "")
        {
            properties["ErrorCode"] = error.Code;
            properties["ErrorDescription"] = error.Description;
            analyticsService.LogException(sender, e, properties, caller);
            return error.ToString();
        }
         
        //Response extensions
        public static Response TraceErrorResponse(this IAnalyticsService analyticsService, object sender, Error error,
            LogSeverity logSeverity = LogSeverity.Error, [CallerMemberName] string caller = "")
        {
            analyticsService.TraceError(sender, error, logSeverity, caller);
            return Response.Failure(error);
        }
        public static Response TraceErrorResponse(this IAnalyticsService analyticsService, object sender, Error error,
            Dictionary<string, object> properties, LogSeverity logSeverity = LogSeverity.Error, [CallerMemberName] string caller = "")
        {
            analyticsService.TraceError(sender, error, properties, logSeverity, caller);
            return Response.Failure(error);
        } 
        public static Response<T> TraceErrorResponse<T>(this IAnalyticsService analyticsService, object sender, Error error,
            LogSeverity logSeverity = LogSeverity.Error, [CallerMemberName] string caller = "")
        {
            analyticsService.TraceError(sender, error, logSeverity, caller);
            return Response.Failure<T>(error);
        }
        public static Response<T> TraceErrorResponse<T>(this IAnalyticsService analyticsService, object sender, Error error, 
            Dictionary<string, object> properties, LogSeverity logSeverity = LogSeverity.Error, [CallerMemberName] string caller = "")
        {
            analyticsService.TraceError(sender, error, properties, logSeverity, caller);
            return Response.Failure<T>(error);
        } 

        public static Response LogExceptionResponse(this IAnalyticsService analyticsService, object sender, Exception e, Error error, 
            [CallerMemberName] string caller = "")
        {
            analyticsService.LogException(sender, e, new Dictionary<string, object>
            {
                {"ErrorCode", error.Code },
                {"ErrorDescription", error.Description},
            }, caller);
            return Response.Failure(error);
        }
        public static Response LogExceptionResponse(this IAnalyticsService analyticsService, object sender, Exception e, Error error,  
            Dictionary<string, object> properties, [CallerMemberName] string caller = "")
        {
            properties["ErrorCode"] = error.Code;
            properties["ErrorDescription"] = error.Description;
            analyticsService.LogException(sender, e, properties, caller);
            return Response.Failure(error);
        }
        public static Response<T> LogExceptionResponse<T>(this IAnalyticsService analyticsService, object sender, Exception e, Error error, 
            [CallerMemberName] string caller = "")
        {
            analyticsService.LogException(sender, e,  new Dictionary<string, object>
            {
                {"ErrorCode", error.Code },
                {"ErrorDescription", error.Description},
            }, caller);
            return Response.Failure<T>(error);
        }
        public static Response<T> LogExceptionResponse<T>(this IAnalyticsService analyticsService, object sender, Exception e, Error error,
            Dictionary<string, object> properties, [CallerMemberName] string caller = "")
        {
            properties["ErrorCode"] = error.Code;
            properties["ErrorDescription"] = error.Description;
            analyticsService.LogException(sender, e, properties, caller);
            return Response.Failure<T>(error);
        }

    }
}