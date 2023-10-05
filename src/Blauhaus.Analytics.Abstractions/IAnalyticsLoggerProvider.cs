namespace Blauhaus.Analytics.Abstractions;

public interface IAnalyticsLoggerProvider
{
    IAnalyticsLogger CreateLogger(string categoryName);
}