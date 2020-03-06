using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Analytics.Xamarin._Ioc;
using Blauhaus.Common.ValueObjects.BuildConfigs;
using Blauhaus.Ioc.Abstractions;
using Blauhaus.Ioc.DryIocService;
using Prism;
using Prism.DryIoc;
using Prism.Ioc;
using Samples.Xamarin.Prism._Config;
using Samples.Xamarin.Prism.Services;
using Samples.Xamarin.Prism.ViewModels;
using Samples.Xamarin.Prism.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Samples.Xamarin.Prism
{
    public partial class App
    {
        private IIocService _iocService;

        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer)
        {
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            _iocService = new DryIocService(containerRegistry.GetContainer());
            _iocService.RegisterType<NumberGenerator>();
            _iocService.RegisterXamarinAnalyticsService<AnalyticsConfig>();
            RegisterBuildConfig();

            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
        }


        protected override async void OnInitialized()
        {
            InitializeComponent();
            var nav = await NavigationService.NavigateAsync("NavigationPage/MainPage");
            if (!nav.Success) throw nav.Exception;
        }


        private void RegisterBuildConfig()
        {
            #if DEBUG
            _iocService.RegisterInstance<IBuildConfig>(BuildConfig.Debug);
            #else
            _iocService.RegisterInstance<IBuildConfig>(BuildConfig.Release);
            #endif
        }
    }
}
