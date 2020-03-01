using Blauhaus.Analytics.Abstractions.Session;
using Blauhaus.DeviceServices.Abstractions.Application;
using Blauhaus.DeviceServices.Abstractions.DeviceInfo;

namespace Blauhaus.Analytics.Xamarin.SessionFactories
{
    public class XamarinSessionFactory : IAnalyticsSessionFactory
    {
        private readonly IDeviceInfoService _deviceInfoService;
        private readonly IApplicationInfoService _applicationInfoService;

        public XamarinSessionFactory(
            IDeviceInfoService deviceInfoService,
            IApplicationInfoService applicationInfoService)
        {
            _deviceInfoService = deviceInfoService;
            _applicationInfoService = applicationInfoService;
        }

        public IAnalyticsSession CreateSession()
        {
            var session = AnalyticsSession.New;
            session.DeviceId = _deviceInfoService.DeviceUniqueIdentifier;
            session.AppVersion = _applicationInfoService.CurrentVersion;
            return session;
        }
    }
}