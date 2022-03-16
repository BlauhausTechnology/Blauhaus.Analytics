using System;
using Blauhaus.Analytics.Abstractions.Service;
using System.Runtime.CompilerServices;
using Blauhaus.Errors;
using Blauhaus.Responses;
using Microsoft.Extensions.Logging;

namespace Blauhaus.Analytics.Abstractions.Extensions;

public static class LoggerExtensions
{ 
    public static ILogger LogError(this ILogger logger, Error error, Exception? e = null)
    {
        logger.LogError(error.ToString(), e);
        return logger;
    }

    public static Response<T> LogErrorResponse<T>(this ILogger logger, Error error, Exception? e = null)
    {
        logger.LogError(error, e);
        return Response.Failure<T>(error);
    } 
}