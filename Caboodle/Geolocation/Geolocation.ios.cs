using System;
using System.Threading;
using System.Threading.Tasks;
using CoreLocation;
using Foundation;

namespace Microsoft.Caboodle
{
    public static partial class Geolocation
    {
        internal static bool IsSupported
            => CLLocationManager.LocationServicesEnabled;

        static async Task<Location> PlatformLastKnownLocationAsync()
        {
            await Permissions.RequireAsync(PermissionType.LocationWhenInUse).ConfigureAwait(false);

            var mngr = new CLLocationManager();
            var location = mngr.Location;

            return new Location
            {
                Latitude = location.Coordinate.Latitude,
                Longitude = location.Coordinate.Longitude,
                Accuracy = location.HorizontalAccuracy,
                TimestampUtc = ToDate(location.Timestamp)
            };
        }

        static async Task<Location> PlatformLocationAsync(GeolocationRequest request, CancellationToken cancellationToken)
        {
            await Permissions.RequireAsync(PermissionType.LocationWhenInUse).ConfigureAwait(false);

            return null;
        }

        static DateTimeOffset ToDate(NSDate timestamp)
        {
            try
            {
                return new DateTimeOffset((DateTime)timestamp);
            }
            catch
            {
                return DateTimeOffset.UtcNow;
            }
        }
    }
}
