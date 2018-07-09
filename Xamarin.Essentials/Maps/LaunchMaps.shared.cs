using System;
using System.Threading.Tasks;

namespace Xamarin.Essentials
{
    public static partial class LaunchMaps
    {
        public static Task OpenAsync(Location location, MapLaunchOptions options)
            => location != null ? PlatformOpenMapsAsync(location.Latitude, location.Longitude, options)
            : throw new ArgumentNullException(nameof(location));

        public static Task OpenAsync(double latitude, double longitude, MapLaunchOptions options)
            => options != null ? PlatformOpenMapsAsync(latitude, longitude, options)
            : throw new ArgumentNullException(nameof(options));

        public static Task OpenAsync(Placemark placemark, MapLaunchOptions options)
        {
            if (placemark == null)
                throw new ArgumentNullException(nameof(placemark));
            if (options == null)
                throw new ArgumentNullException(nameof(options));
            return PlatformOpenMapsAsync(placemark, options);
        }
    }
}
