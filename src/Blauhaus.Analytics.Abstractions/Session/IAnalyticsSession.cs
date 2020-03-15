using System.Collections.Generic;
using Blauhaus.Common.ValueObjects.DeviceType;
using Blauhaus.Common.ValueObjects.RuntimePlatforms;

namespace Blauhaus.Analytics.Abstractions.Session
{
    public interface IAnalyticsSession
    {
        string Id { get; }                                  // telemetry.Context.Session.Id
        
        string? UserId { get; set; }                        // telemetry.Context.User.AuthenticatedUserId
        string? AccountId { get; set; }                     // telemetry.Context.User.AccountId
        string? DeviceId { get; set; }                      // telemetry.Context.Device.Id
        string? AppVersion { get; set; }                    // telemetry.Context.Component.Version

        IDeviceType DeviceType { get; set; }                // telemetry.Context.Device.Type
        IRuntimePlatform Platform { get; set; }             // telemetry.Context.Device.OperatingSystem
        string OperatingSystemVersion { get; set; }         // Properties
        string Manufacturer { get; set; }                   // telemetry.Context.Device.OemName
        string Model { get; set; }                          // telemetry.Context.Device.Model

        IReadOnlyDictionary<string, string> Properties { get; }
        void SetProperty(string key, string value);
        string Property(string key);
    }
}