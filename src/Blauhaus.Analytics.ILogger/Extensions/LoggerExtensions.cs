using Blauhaus.Errors;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;
using Blauhaus.Responses;

namespace Blauhaus.Analytics.ILogger.Extensions
{
    public static class LoggerExtensions
    {
        public static ILogger<T> LogError<T>(this ILogger<T> logger, Error error, int eventId, string eventName)
        {

            logger.Log(LogLevel.Error, new EventId(eventId, eventName), "{code}: {description}", error.Code, error.Description);

            return logger;
        }

        public static Response<T> TraceErrorResponse<T>(this ILogger<T> logger, Error error, int eventId, string eventName)
        {
            logger.LogError(error, eventId, eventName);
            return Response.Failure<T>(error);
        }
    }
}