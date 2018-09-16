using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Samples.ViewModel
{
    public class ScreenShotViewModel : BaseViewModel
    {
        string uri;

        public string Uri
        {
            get => uri;
            set => SetProperty(ref uri, value);
        }

        public Command ScreenCommand { get; }

        public ScreenShotViewModel() => 
            ScreenCommand = new Command(async () => await ExecuteScreenCommand(), () => !IsBusy);

        async Task ExecuteScreenCommand()
        {
            if (!IsBusy)
            {
                try
                {
                    IsBusy = true;

                    Uri = await ScreenShot.CaptureAsync();
                    Uri = await ScreenShot.CaptureAsync(ScreenOutputType.JPEG);
                    Uri = await ScreenShot.CaptureAsync(ScreenOutputType.PNG, "teste");

                    var stream = await ScreenShot.GetImageBytesAsync();
                    if (stream is null)
                        throw new Exception("Stream nula");
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
