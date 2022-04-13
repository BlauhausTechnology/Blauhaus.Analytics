using System.Collections.Generic;
using Blauhaus.Analytics.Abstractions;
using Blauhaus.Analytics.Common;
using Blauhaus.Analytics.Serilog;
using Blauhaus.DeviceServices.Abstractions.Application;
using Blauhaus.DeviceServices.Abstractions.DeviceInfo;

namespace Blauhaus.Analytics.Xamarin;
public class XamarinAnalyticsContext : InMemoryAnalyticsContext
{
    public XamarinAnalyticsContext(IDeviceInfoService deviceInfoService, IApplicationInfoService applicationInfoService)
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