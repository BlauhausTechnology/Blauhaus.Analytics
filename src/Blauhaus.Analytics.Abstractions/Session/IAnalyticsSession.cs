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

        IReadOnlyDictionary<string, string> Properties { get; }
        void SetProperty(string key, string value);
        string Property(string key);
    }
}