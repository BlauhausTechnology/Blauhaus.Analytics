using System;
using Blauhaus.Analytics.Xamarin._Ioc;
using Blauhaus.Common.ValueObjects.BuildConfigs;
using Blauhaus.Ioc.Abstractions;
using Blauhaus.Ioc.DotNetCoreIocService;
using Microsoft.Extensions.DependencyInjection;
using Samples.Xamarin._Config;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Samples.Xamarin
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            var iocService = new DotNetCoreIocService(new ServiceCollection());
            
            RegisterBuildConfig(iocService);
            iocService.RegisterType<NumberGenerator>();
            iocService.RegisterXamarinAnalyticsService<AnalyticsConfig>();


            MainPage = new MainView(iocService);
        }

        private void RegisterBuildConfig(IIocService iocService)
        {
            #if DEBUG
            iocService.RegisterInstance(BuildConfig.Debug);
            #else
            iocService.RegisterInstance(BuildConfig.Release);
            #endif
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
