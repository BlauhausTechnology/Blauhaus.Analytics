using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Blauhaus.Analytics.Abstractions.Attributes;
using Blauhaus.Errors;
using Blauhaus.Responses;
using Microsoft.Extensions.Logging;

namespace Blauhaus.Analytics.Abstractions.Extensions;

public static class LoggerExtensions
{

    public static ILogger LogError(this ILogger logger, Error error, Exception? e = null, 
        [CallerMemberName] string memberName = "", [CallerLineNumber] int sourceLineNumber = 0)
    {
        logger.LogError(e, " *** {Code} *** {Description} in {Caller} on line {LineNumber}", error.Code, error.Description, memberName, sourceLineNumber);
        return logger;
    }

    public static Response<T> LogErrorResponse<T>(this ILogger logger, Error error, Exception? e = null,
        [CallerMemberName] string memberName = "", [CallerLineNumber] int sourceLineNumber = 0)
    {
        LogError(logger, error, e, memberName, sourceLineNumber);
        return Response.Failure<T>(error);
    } 
    public static Response LogErrorResponse(this ILogger logger, Error error, Exception? e = null,
        [CallerMemberName] string memberName = "", [CallerLineNumber] int sourceLineNumber = 0)
    {
        LogError(logger, error, e, memberName, sourceLineNumber);
        return Response.Failure(error);
    }
    
    [MessageFormatMethod("messageTemplate")]
    public static IDisposable LogTimer(this ILogger logger, string messageTemplate, params object[] args)
    {
        var newArgs = new object[args.Length+1];
        for (var i = 0; i < args.Length; i++)
        {
            newArgs[i] = args[i];
        }
        return new LoggerTimer(duration =>
        {
            newArgs[newArgs.Length - 1] = Math.Round(duration.TotalMilliseconds,2);
            messageTemplate += " in {Duration}ms";
            logger.Log(LogLevel.Debug, messageTemplate, newArgs);
        });
    }

}




public class LoggerTimer : IDisposable
{
    private readonly Action<TimeSpan> _onStopAction;

    private readonly Stopwatch _stopwatch = new();
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