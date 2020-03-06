using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Blauhaus.Ioc.Abstractions;
using Xamarin.Forms;

namespace Samples.Xamarin
{
    public class MainViewModel : INotifyPropertyChanged
    {

        public MainViewModel(IIocService iocService)
        {
            var numberGenerator = iocService.Resolve<NumberGenerator>();
            ChangeNumberCommand = new Command(async () =>
            {
                Number = await numberGenerator.GenerateAsync();
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
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Number)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}