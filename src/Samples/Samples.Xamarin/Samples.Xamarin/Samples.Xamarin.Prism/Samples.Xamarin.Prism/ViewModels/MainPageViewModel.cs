using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Blauhaus.Analytics.Abstractions.Service;
using Samples.Xamarin.Prism.Services;
using Xamarin.Forms;

namespace Samples.Xamarin.Prism.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public MainPageViewModel(INavigationService navigationService, IAnalyticsService analyticsService, NumberGenerator numberGenerator)
            : base(navigationService)
        {
            Title = "Main Page";

            ChangeNumberCommand = new Command(async () =>
            {
                using (var _ = analyticsService.StartOperation(this, "Generate Number"))
                {
                    analyticsService.Trace(this, "Starting number generation", LogSeverity.Information);
                    Number = await numberGenerator.GenerateAsync();
                }
            });
        }

        
        public ICommand ChangeNumberCommand { get; }


        private int _number;
        public int Number
        {
            get => _number;
            set
            {
                _number = value;
                RaisePropertyChanged();
            }
        }
    }
}
