using System;
using System.Diagnostics;
using Blauhaus.Errors;
using Blauhaus.Responses;
using Microsoft.Extensions.Logging;

namespace Blauhaus.Analytics.Abstractions.Extensions;

public static class LoggerExtensions
{ 
    public static ILogger LogError(this ILogger logger, Error error, Exception? e = null)
    {
        logger.LogError(e, error.ToString());
        return logger;
    }

    public static Response<T> LogErrorResponse<T>(this ILogger logger, Error error, Exception? e = null)
    {
        LogError(logger, error, e);
        return Response.Failure<T>(error);
    } 
    public static Response LogErrorResponse(this ILogger logger, Error error, Exception? e = null)
    {
        LogError(logger, error, e);
        return Response.Failure(error);
    }

    public static IDisposable LogTimer(this ILogger logger, string messageTemplate, params object[] args)
    {
        var newArgs = new object[args.Length+1];
        for (var i = 0; i < args.Length-1; i++)
        {
            newArgs[i] = args[i];
        }
        return new LoggerTimer(duration =>
        {

            newArgs[newArgs.Length - 1] = duration;
            messageTemplate += " Duration: {Duration}";
            logger.Log(LogLevel.Debug, messageTemplate, newArgs);
        });
    }

}




public class LoggerTimer : IDisposable
{
    private readonly Action<TimeSpan> _onStopAction;

    private readonly Stopwatch _stopwatch = new Stopwatch();
    public LoggerTimer(Action<TimeSpan> onStopAction)
    {
        Id = Guid.NewGuid();
        _onStopAction = onStopAction;
        _stopwatch.Start();
    }

    public Guid Id { get; set; }

    public void Dispose()
    {
        Stop();
        GC.SuppressFinalize(this);
    }

    public void Stop()
    {
        if (_stopwatch.IsRunning)
        {
            _stopwatch.Stop(); 
            _onStopAction.Invoke(_stopwatch.Elapsed);
        }
    }
}