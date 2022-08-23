using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Blauhaus.Auth.Abstractions.Errors;
using Blauhaus.Errors;
using Blauhaus.Responses;
using Microsoft.Extensions.Logging;

namespace Blauhaus.Analytics.Abstractions.Extensions;

public static class AnalyticsLoggerExtensions
{
    public static Response LogNetworkError(this IAnalyticsLogger logger, Exception exception, string? url = null, object? payload = null, string? responseString = null)
    {
        return logger
            .SetValues(url, payload, responseString)
            .LogErrorResponse(exception.ExctractNetworkError(), exception);
    }
    public static Response<T> LogNetworkError<T>(this IAnalyticsLogger logger, Exception exception, string? url = null, object? payload = null, string? responseString = null)
    {
        return logger
            .SetValues(url, payload, responseString)
            .LogErrorResponse<T>(exception.ExctractNetworkError(), exception);
    }

    private static IAnalyticsLogger SetValues(this IAnalyticsLogger logger, string? url, object? payload, string? responseString)
    {
        if (url is not null) logger.SetValue("Url", url);
        if (payload is not null) logger.SetValue("Payload", JsonSerializer.Serialize(payload));
        if (responseString is not null) logger.SetValue("Response", responseString);
        return logger;
    }
    private static Error ExctractNetworkError(this Exception exception)
    {
        if (exception is ErrorException errorException)
        {
            return errorException.Error;
        }

        var message = exception.Message;
        
        if (exception is HttpRequestException)
        {
            if (message.Contains("401") || message.Contains("403"))
                return AuthError.NotAuthorized;
            
            if(message.Contains("500"))
                return Error.Unexpected();
            
            return Error.Unexpected($"Unexpected Http error: {message}");
        }
        
        var error = Error.Unexpected("Unable to complete network request");

        if (exception is TaskCanceledException)
        {
            error = message.StartsWith("The request was canceled due to the configured HttpClient.Timeout") 
                ? Error.Timeout 
                : Error.Cancelled;
        }

        return error;
    }

}