using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Samples.ViewModel
{
    public class LaunchMapsViewModel : BaseViewModel
    {
        string longitude = "0";

        public string Longitude { get => longitude; set => SetProperty(ref longitude, value); }

        string latitude = "0";

        public string Latitude { get => latitude; set => SetProperty(ref latitude, value); }

        public ICommand LaunchMapsCommand { get; }

        public ICommand LaunchPlacemarkCommand { get; }

        public LaunchMapsViewModel()
        {
            LaunchMapsCommand = new Command(OpenLocation);
            LaunchPlacemarkCommand = new Command(OpenPlacemark);
        }

        async void OpenLocation()
        {
            await LaunchMaps.OpenAsync(double.Parse(Latitude), double.Parse(Longitude), new MapLaunchOptions() { Name = "Bundestag" });
        }

        async void OpenPlacemark()
        {
            var placemark = new Placemark()
            {
                Locality = "Berlin",
                AdminArea = "Berlin",
                CountryName = "Germany",
                Thoroughfare = "Platz der Republik 1"
            };
            await LaunchMaps.OpenAsync(placemark, new MapLaunchOptions() { Name = "Bundestag" });
        }
    }
}
