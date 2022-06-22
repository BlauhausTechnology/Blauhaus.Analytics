using Blauhaus.Analytics.Abstractions;
using Orleans;

namespace Blauhaus.Analytics.Serilog.Orleans;

public class AnalyticsGrainFilter : IIncomingGrainCallFilter
{
    private readonly IAnalyticsLogger<AnalyticsGrainFilter> _logger;

    public AnalyticsGrainFilter(IAnalyticsLogger<AnalyticsGrainFilter> logger)
    {
        _logger = logger;
    }

    public Task Invoke(IIncomingGrainCallContext context)
    {
        //this will force the analytics context to get the analytics headers from the Orleans RequestContext and write them to the logger scope
        _logger.BeginScope();
        return Task.CompletedTask;
    }
}