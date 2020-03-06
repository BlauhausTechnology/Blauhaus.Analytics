using System.ComponentModel;
using Xamarin.Forms;

namespace Samples.Xamarin
{
    [DesignTimeVisible(false)]
    public partial class MainView : ContentPage
    {
        public MainView()
        {
            BindingContext = new MainViewModel(new NumberGenerator());


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
    }
}
