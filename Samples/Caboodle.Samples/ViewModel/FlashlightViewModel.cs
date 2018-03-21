using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Caboodle;
using Xamarin.Forms;

namespace Caboodle.Samples.ViewModel
{
    public class FlashlightViewModel : BaseViewModel
    {
        public FlashlightViewModel()
        {
            ToggleCommand = new Command(OnTurnOnOffAsync);

            // switchFlashlight
        }

        public ICommand ToggleCommand { get; }

        string textflashlight = "Off";

        public string TextFlashlight
        {
            get => textflashlight;
            set => SetProperty(ref textflashlight, value);
        }

        async void OnTurnOnOffAsync()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            try
            {
                if (TextFlashlight == "On")
                {
                    await Task.Run(() => Flashlight.Off());
                    TextFlashlight = "Off";
                }
                else if (TextFlashlight == "Off")
                {
                    await Task.Run(() => Flashlight.On());
                    TextFlashlight = "On";
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                IsBusy = false;
            }

            return;
        }
    }
}
