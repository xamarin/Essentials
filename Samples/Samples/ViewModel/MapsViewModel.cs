using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Samples.ViewModel
{
    public class MapsViewModel : BaseViewModel
    {
        string longitude;

        public string Longitude { get => longitude; set => SetProperty(ref longitude, value); }

        string latitude;

        public string Latitude { get => latitude; set => SetProperty(ref latitude, value); }

        public ICommand LaunchMapsCommand { get; }

        public MapsViewModel()
        {
            LaunchMapsCommand = new Command(OpenLocation);
        }

        async void OpenLocation()
        {
            await Map.OpenMapsAsync(double.Parse(Latitude), double.Parse(Longitude), new MapLaunchOptions());
        }
    }
}
