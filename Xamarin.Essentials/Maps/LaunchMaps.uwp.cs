using System;
using System.Globalization;
using System.Threading.Tasks;

namespace Xamarin.Essentials
{
    public static partial class LaunchMaps
    {
        internal static Task PlatformOpenMapsAsync(double latitude, double longitude, MapLaunchOptions options)
        => LaunchUri(new Uri(
                $"bingmaps:?collection=point." +
                $"{latitude.ToString(CultureInfo.InvariantCulture)}" +
                $"_" +
                $"{longitude.ToString(CultureInfo.InvariantCulture)}" +
                $"_" +
                $"{options.Name}"));

        internal static Task PlatformOpenMapsAsync(Placemark placemark, MapLaunchOptions options)
        {
            placemark = placemark.Escape();
            var uri = new Uri(
                $"bingmaps:?where=" +
                $"{placemark.Thoroughfare}" +
                $"%20{placemark.Locality}" +
                $"%20{placemark.AdminArea}" +
                $"%20{placemark.CountryName}");
            return LaunchUri(uri);
        }

        static Task LaunchUri(Uri mapsUri)
        {// Use launcher api once it is available
            return Windows.System.Launcher.LaunchUriAsync(mapsUri).AsTask();
        }
    }
}
