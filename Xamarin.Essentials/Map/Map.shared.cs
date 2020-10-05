using System;
using System.Threading.Tasks;

namespace Xamarin.Essentials
{
    public static partial class Map
    {
        public static Task OpenAsync(Location location) =>
            OpenAsync(location, new MapLaunchOptions());

        public static Task OpenAsync(Location location, MapLaunchOptions options)
        {
            if (location == null)
                ThrowHelper.ThrowArgumentNullException(nameof(location));

            if (options == null)
                ThrowHelper.ThrowArgumentNullException(nameof(options));

            return PlatformOpenMapsAsync(location.Latitude, location.Longitude, options);
        }

        public static Task OpenAsync(double latitude, double longitude) =>
            OpenAsync(latitude, longitude, new MapLaunchOptions());

        public static Task OpenAsync(double latitude, double longitude, MapLaunchOptions options)
        {
            if (options == null)
                ThrowHelper.ThrowArgumentNullException(nameof(options));

            return PlatformOpenMapsAsync(latitude, longitude, options);
        }

        public static Task OpenAsync(Placemark placemark) =>
            OpenAsync(placemark, new MapLaunchOptions());

        public static Task OpenAsync(Placemark placemark, MapLaunchOptions options)
        {
            if (placemark == null)
                ThrowHelper.ThrowArgumentNullException(nameof(placemark));

            if (options == null)
                ThrowHelper.ThrowArgumentNullException(nameof(options));

            return PlatformOpenMapsAsync(placemark, options);
        }
    }
}
