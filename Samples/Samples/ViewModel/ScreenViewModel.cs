using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Samples.ViewModel
{
    public class ScreenViewModel : BaseViewModel
    {
        string uri;

        public string Uri
        {
            get => uri;
            set => SetProperty(ref uri, value);
        }

        public Command ScreenCommand { get; }

        public ScreenViewModel()
        {
            ScreenCommand = new Command(async () => await ExecuteScreenCommand(), () => !IsBusy);
        }

        async Task ExecuteScreenCommand()
        {
            if (!IsBusy)
            {
                try
                {
                    IsBusy = true;
                    Uri = await ScreenShot.CaptureAsync();
                }
                catch (Exception ex)
                {
                    await App.Current.MainPage.DisplayAlert("Erro", $"Erro:{ex.Message}", "Ok");
                }
                finally
                {
                    IsBusy = false;
                }
            }
            return;
        }
    }
}
