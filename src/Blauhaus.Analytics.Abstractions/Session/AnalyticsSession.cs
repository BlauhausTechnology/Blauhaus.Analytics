using System;
using System.Collections.Generic;

namespace Blauhaus.Analytics.Abstractions.Session
{
    public class AnalyticsSession : IAnalyticsSession
    {
        private AnalyticsSession(string id)
        {
            Id = id;
        }

        public string Id { get; }
        public string? UserId { get; set; }
        public string? AccountId { get; set; }
        public string? DeviceId { get; set; }
        public string? AppVersion { get; set; }
        public Dictionary<string, string> Properties { get; } = new Dictionary<string, string>();

        public static AnalyticsSession Empty => new AnalyticsSession(string.Empty);
        public static AnalyticsSession New => new AnalyticsSession(Guid.NewGuid().ToString());
        public static AnalyticsSession FromExisting(string sessionId) => new AnalyticsSession(sessionId);
    }
}