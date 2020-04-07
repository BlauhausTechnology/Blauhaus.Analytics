using Blauhaus.DeviceServices.Abstractions.DeviceInfo;
using Xamarin.Forms;

namespace Samples.Xamarin.Prism.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage(IDeviceInfoService deviceInfoService)
        {

            var t = deviceInfoService.AppDataFolder;
            var sd3 = deviceInfoService.DeviceUniqueIdentifier;
            InitializeComponent();
        }
    }
}