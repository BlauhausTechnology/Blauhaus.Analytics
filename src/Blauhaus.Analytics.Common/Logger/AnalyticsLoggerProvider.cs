using Blauhaus.Analytics.Abstractions;
using Microsoft.Extensions.Logging;

namespace Blauhaus.Analytics.Common.Logger;

public class AnalyticsLoggerProvider : IAnalyticsLoggerProvider
{
    private readonly ILoggerProvider _loggerProvider;
    private readonly IAnalyticsContext _context;

    public AnalyticsLoggerProvider(
        ILoggerProvider loggerProvider, 
        IAnalyticsContext context)
    {
        _loggerProvider = loggerProvider;
        _context = context;
    }

    public IAnalyticsLogger CreateLogger(string categoryName)
    {
        return new AnalyticsLogger(categoryName, _loggerProvider, _context);
    }

    public void Dispose()
    {
        _loggerProvider.Dispose();
    }
     
}