using System;
using System.Net;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Essentials.SaveToGalery;
using Xamarin.Forms;

namespace Samples.ViewModel
{
    public class SaveToGaleryViewModel : BaseViewModel
    {
        public SaveToGaleryViewModel()
            => SaveCommand = new Command(Save);

        public ICommand SaveCommand { get; }

        public string ImageUrl
            => "https://raw.githubusercontent.com/xamarin/Essentials/main/Assets/xamarin.essentials_128x128.png";

        async void Save()
        {
            try
            {
                using var client = new WebClient();
                var data = await client.DownloadDataTaskAsync(ImageUrl);
                await SaveToGalery.SaveImageAsync(data, "hhhh.png");
            }
            catch (Exception ex)
            {
                await DisplayAlertAsync(ex.Message);
            }
        }
    }
}
