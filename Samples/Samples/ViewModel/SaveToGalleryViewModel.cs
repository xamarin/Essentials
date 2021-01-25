using System;
using System.Net;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Samples.ViewModel
{
    public class SaveToGalleryViewModel : BaseViewModel
    {
        public SaveToGalleryViewModel()
        {
            SavevPngCommand = new Command(() => Save(PngUrl, "essential.png"));
            SaveJpgCommand = new Command(() => Save(JpgUrl, "Lomonosov.jpg"));
            SaveGifCommand = new Command(() => Save(GifUrl, "test.gif"));
            SaveMp4Command = new Command(() => Save(Mp4Url, "essential.mov"));
        }

        public ICommand SavevPngCommand { get; }

        public ICommand SaveJpgCommand { get; }

        public ICommand SaveGifCommand { get; }

        public ICommand SaveMp4Command { get; }

        public string PngUrl
            => "https://raw.githubusercontent.com/xamarin/Essentials/main/Assets/xamarin.essentials_128x128.png";

        public string JpgUrl
            => "https://raw.githubusercontent.com/dimonovdd/Essentials/featureSaveToGalery/Assets/SaveToGaleryTestPhoto.jpg";

        public string GifUrl
            => "https://i.gifer.com/769R.gif";

        public string Mp4Url
            => "https://xvid.ru/play/tests/qt7.mov";

        async void Save(string url, string name)
        {
            try
            {
                using var client = new WebClient();
                var data = await client.DownloadDataTaskAsync(url);
                if (url == Mp4Url)
                    await SaveToGallery.SaveVideoAsync(data, name, "Essentials");
                else
                    await SaveToGallery.SaveImageAsync(data, name, "Essentials");
            }
            catch (Exception ex)
            {
                await DisplayAlertAsync(ex.Message);
            }
        }
    }
}
