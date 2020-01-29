using System;
using System.Collections.Generic;

namespace Blauhaus.Analytics.Abstractions.Session
{
    public class AnalyticsSession
    {
        private AnalyticsSession(string id, string? userId, string? accountId, string? deviceId, Dictionary<string, string>? properties = null)
        {
            Id = id;
            UserId = userId;
            AccountId = accountId;
            DeviceId = deviceId;
            Properties = properties ?? new Dictionary<string, string>();
        }

        public string Id { get; }
        public string? UserId { get; }
        public string? AccountId { get; }
        public string? DeviceId { get; }
        public Dictionary<string, string> Properties { get; } 

        public static AnalyticsSession Empty => new AnalyticsSession(string.Empty, null, null, null, null);
        public static AnalyticsSession New => new AnalyticsSession(Guid.NewGuid().ToString(), null, null, null, null);
        public static AnalyticsSession FromRequest(string sessionId) => new AnalyticsSession(sessionId, null, null, null, null);
    }
}