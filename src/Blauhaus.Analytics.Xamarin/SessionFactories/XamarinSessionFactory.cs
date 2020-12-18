using Blauhaus.Analytics.Abstractions.Session;
using Blauhaus.DeviceServices.Abstractions.Application;
using Blauhaus.DeviceServices.Abstractions.DeviceInfo;

namespace Blauhaus.Analytics.Xamarin.SessionFactories
{
    public class XamarinSessionFactory : IAnalyticsSessionFactory
    {
        private readonly IDeviceInfoService _deviceInfoService;
        private readonly IApplicationInfoService _applicationInfoService;

        private AnalyticsSession _session;

        public XamarinSessionFactory(
            IDeviceInfoService deviceInfoService,
            IApplicationInfoService applicationInfoService)
        {
            _deviceInfoService = deviceInfoService;
            _applicationInfoService = applicationInfoService;
        }

        public IAnalyticsSession CreateSession()
        {
            _session = AnalyticsSession.New;
            _session.DeviceId = _deviceInfoService.DeviceUniqueIdentifier;
            _session.AppVersion = _applicationInfoService.CurrentVersion;
            _session.DeviceType = _deviceInfoService.Type;
            _session.Platform = _deviceInfoService.Platform;
            _session.OperatingSystemVersion = _deviceInfoService.OperatingSystemVersion;
            _session.Manufacturer = _deviceInfoService.Manufacturer;
            _session.Model = _deviceInfoService.Model;

            return _session;
        }
    }
}