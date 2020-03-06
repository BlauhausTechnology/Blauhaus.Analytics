using System;
using System.Collections.Generic;
using Blauhaus.Common.ValueObjects.DeviceType;
using Blauhaus.Common.ValueObjects.RuntimePlatforms;

namespace Blauhaus.Analytics.Abstractions.Session
{
    public class AnalyticsSession : IAnalyticsSession
    {
        private readonly Dictionary<string, string> _properties = new Dictionary<string, string>();

        private AnalyticsSession(string id)
        {
            Id = id;
        }

        public string Id { get; }
        public string? UserId { get; set; }
        public string? AccountId { get; set; }
        public string? DeviceId { get; set; }
        public string? AppVersion { get; set; }
        public IDeviceType DeviceType { get; set; }
        public IRuntimePlatform Platform { get; set; }
        public string OperatingSystemVersion { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public IReadOnlyDictionary<string, string> Properties => _properties;
        public void SetProperty(string key, string value)
        {
            _properties[key] = value;
        }

        public string Property(string key)
        {
            return _properties.TryGetValue(key, out var value) 
                ? value 
                : string.Empty;
        }

        public static AnalyticsSession Empty => new AnalyticsSession(string.Empty);
        public static AnalyticsSession New => new AnalyticsSession(Guid.NewGuid().ToString());
        public static AnalyticsSession FromExisting(string sessionId) => new AnalyticsSession(sessionId);
    }
}