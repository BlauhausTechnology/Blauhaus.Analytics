using Blauhaus.DeviceServices.Abstractions.Application;
using Blauhaus.DeviceServices.Abstractions.DeviceInfo;

namespace Blauhaus.Analytics.Serilog.Maui;

public class MauiAnalyticsContext : SerilogAnalyticsContext
{
    public MauiAnalyticsContext(
        IDeviceInfoService deviceInfoService, 
        IApplicationInfoService applicationInfoService)
    {
        SetValues(new Dictionary<string, object>
        {
            ["DeviceIdentifier"] = deviceInfoService.DeviceUniqueIdentifier,
            ["AppVersion"] = applicationInfoService.CurrentVersion,
            ["DeviceType"] = deviceInfoService.Type,
            ["Platform"] = deviceInfoService.Platform,
            ["OperatingSystemVersion"] = deviceInfoService.OperatingSystemVersion,
            ["Manufacturer"] = deviceInfoService.Manufacturer,
            ["Model"] = deviceInfoService.Model,
        }); 
    }
}