using System.Collections.Generic;

namespace Blauhaus.Analytics.Abstractions.Session
{
    public interface IAnalyticsSession
    {
        string Id { get; }
        string? UserId { get; set; }
        string? AccountId { get; set; }
        string? DeviceId { get; set; }
        string? AppVersion { get; set; }
        Dictionary<string, string> Properties { get; }
    }
}