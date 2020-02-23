using Blauhaus.DeviceServices.Abstractions.Application;
using Blauhaus.DeviceServices.Abstractions.DeviceInfo;

namespace Blauhaus.Analytics.Xamarin.Service
{
    public class DefaultDeviceService : IDeviceInfoService, IApplicationInfoService
    {
        public string DeviceUniqueIdentifier { get; }= string.Empty;
        public string AppDataFolder { get; }= string.Empty;
        public string CurrentVersion { get; } = string.Empty;
        public Platform Platform { get; } = Platform.Unknown;
    }
}