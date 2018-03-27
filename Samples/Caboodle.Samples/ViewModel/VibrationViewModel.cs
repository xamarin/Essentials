using System.Windows.Input;
using Microsoft.Caboodle;
using Xamarin.Forms;

namespace Caboodle.Samples.ViewModel
{
    public class VibrationViewModel : BaseViewModel
    {
        public VibrationViewModel()
        {
            VibrateCommand = new Command(OnVibrate);
        }

        public ICommand VibrateCommand { get; }

        void OnVibrate() => Vibration.Vibrate();
    }
}
