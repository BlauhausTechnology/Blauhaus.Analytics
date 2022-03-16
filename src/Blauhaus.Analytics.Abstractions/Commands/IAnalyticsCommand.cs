using System.Collections.Generic;

namespace Blauhaus.Analytics.Abstractions.Commands;

public interface IAnalyticsCommand
{
    public Dictionary<string, object> Properties { get; }
}