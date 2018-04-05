using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace Microsoft.Caboodle
{
    public static partial class Geolocation
    {
        static async Task<Location> PlatformLastKnownLocationAsync()
        {
            // no need for permissions as AllowFallbackToConsentlessPositions
            // will allow the device to return a location regardless

            var geolocator = new Geolocator
            {
                DesiredAccuracy = PositionAccuracy.Default,
            };
            geolocator.AllowFallbackToConsentlessPositions();

            var location = await geolocator.GetGeopositionAsync().AsTask().ConfigureAwait(false);

            if (location?.Coordinate == null)
                return null;

            return new Location
            {
                Latitude = location.Coordinate.Point.Position.Latitude,
                Longitude = location.Coordinate.Point.Position.Longitude,
                TimestampUtc = location.Coordinate.Timestamp,
                Accuracy = location.Coordinate.Accuracy
            };
        }

        static async Task<Location> PlatformLocationAsync(GeolocationRequest request, CancellationToken cancellationToken)
        {
            await Permissions.RequireAsync(PermissionType.LocationWhenInUse).ConfigureAwait(false);

            var geolocator = new Geolocator
            {
                DesiredAccuracyInMeters = request.DesiredAccuracyInMeters
            };

            cancellationToken = Utils.TimeoutToken(cancellationToken, request.Timeout);

            var location = await geolocator.GetGeopositionAsync().AsTask(cancellationToken).ConfigureAwait(false);

            if (location?.Coordinate == null)
                return null;

            return new Location
            {
                Latitude = location.Coordinate.Point.Position.Latitude,
                Longitude = location.Coordinate.Point.Position.Longitude,
                TimestampUtc = location.Coordinate.Timestamp,
                Accuracy = location.Coordinate.Accuracy
            };
        }
    }
}
