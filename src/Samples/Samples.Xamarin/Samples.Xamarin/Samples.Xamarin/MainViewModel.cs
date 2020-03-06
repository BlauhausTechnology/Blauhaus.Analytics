using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;

namespace Samples.Xamarin
{
    public class MainViewModel : INotifyPropertyChanged
    {

        public MainViewModel()
        {
            ChangeNumberCommand = new Command(async () =>
            {

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