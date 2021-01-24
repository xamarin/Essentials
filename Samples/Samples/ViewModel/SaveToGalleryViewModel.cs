using System;
using System.Net;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Essentials.SaveToGallery;
using Xamarin.Forms;

namespace Samples.ViewModel
{
    public class SaveToGalleryViewModel : BaseViewModel
    {
        public SaveToGalleryViewModel()
            => SaveCommand = new Command<string>(Save);

        public ICommand SaveCommand { get; }

        public string PngUrl
            => "https://raw.githubusercontent.com/xamarin/Essentials/main/Assets/xamarin.essentials_128x128.png";

        public string JpgUrl
            => "https://raw.githubusercontent.com/dimonovdd/Essentials/featureSaveToGalery/Assets/SaveToGaleryTestPhoto.jpg";

        async void Save(string type)
        {
            try
            {
                using var client = new WebClient();
                var data = await client.DownloadDataTaskAsync(type == "png" ? PngUrl : JpgUrl);
                await SaveToGallery.SaveImageAsync(data, "hhhh.png");
            }
            catch (Exception ex)
            {
                await DisplayAlertAsync(ex.Message);
            }
        }
    }
}
