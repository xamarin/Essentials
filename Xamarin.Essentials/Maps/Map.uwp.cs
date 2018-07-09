using System;
using System.Globalization;
using System.Threading.Tasks;

namespace Xamarin.Essentials
{
    public static partial class Map
    {
        internal static Task PlatformOpenMapsAsync(double latitude, double longitude, MapLaunchOptions options)
        {
            var uri = CreatePointUri(latitude, longitude, options);
            return LaunchUri(uri);
        }

        internal static Task PlatformOpenMapsAsync(Placemark placemark, MapLaunchOptions options)
        {
            var uri = CreatePlacemarkUri(placemark, options);
            return LaunchUri(uri);
        }

        static async Task LaunchUri(Uri mapsUri)
        {// Use launcher api once it is available
            await Windows.System.Launcher.LaunchUriAsync(mapsUri);
        }

        static Uri CreatePlacemarkUri(Placemark placemark, MapLaunchOptions options)
        {
            placemark = placemark.Escape();
            var uri = new Uri(
                $"bingmaps:?where=" +
                $"{placemark.Thoroughfare}" +
                $"%20{placemark.Locality}" +
                $"%20{placemark.AdminArea}" +
                $"%20{placemark.CountryName}");
            return uri;
        }

        static Uri CreatePointUri(double latitude, double longitude, MapLaunchOptions options)
        {
            var uri = new Uri(
                $"bingmaps:?collection=point." +
                $"{latitude.ToString(CultureInfo.InvariantCulture)}" +
                $"_" +
                $"{longitude.ToString(CultureInfo.InvariantCulture)}" +
                $"_" +
                $"{options.Name}");
            return uri;
        }
    }
}
