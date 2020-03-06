using System.ComponentModel;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Ioc.Abstractions;
using Blauhaus.Ioc.DotNetCoreIocService;
using Xamarin.Forms;

namespace Samples.Xamarin
{
    [DesignTimeVisible(false)]
    public partial class MainView : ContentPage
    {
        private IAnalyticsService _analytics;

        public MainView(IIocService iocService)
        {
            BindingContext = new MainViewModel(iocService);
            _analytics = iocService.Resolve<IAnalyticsService>();

            var label = new Label();
            label.SetBinding(Label.TextProperty, new Binding(nameof(MainViewModel.Number)));

            var button = new Button{Text = "Click Me!"};
            button.SetBinding(Button.CommandProperty, new Binding(nameof(MainViewModel.ChangeNumberCommand)));

            Content = new StackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                Spacing = 30,

                Children =
                {
                    label,
                    button
                }
            };
        }

        protected override void OnAppearing()
        {
            using (var _ = _analytics.StartPageViewOperation(this, "Main View"))
            {
                base.OnAppearing();
            }
        }
    }
}
