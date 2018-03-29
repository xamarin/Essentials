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
            var location = await geolocator.GetGeopositionAsync();

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
                DesiredAccuracyInMeters = ToMeters(request.DesiredAccuracy)
            };
            var location = await geolocator.GetGeopositionAsync();

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

        static uint ToMeters(GeolocationAccuracy accuracy)
        {
            switch (accuracy)
            {
                case GeolocationAccuracy.Lowest:
                    return 3000;
                case GeolocationAccuracy.Low:
                    return 1000;
                case GeolocationAccuracy.Medium:
                    return 100;
                case GeolocationAccuracy.High:
                    return 10;
                case GeolocationAccuracy.Best:
                    return 1;
                default:
                    return 100;
            }
        }
    }
}
