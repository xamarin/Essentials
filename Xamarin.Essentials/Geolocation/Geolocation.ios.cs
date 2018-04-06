using System;
using System.Linq;
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

            var manager = new CLLocationManager();
            var location = manager.Location;

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

            var manager = new CLLocationManager();

            var tcs = new TaskCompletionSource<CLLocation>(manager);

            var listener = new SingleLocationListener();
            listener.LocationHandler += HandleLocation;

            cancellationToken = Utils.TimeoutToken(cancellationToken, request.Timeout);
            cancellationToken.Register(Cancel);

            manager.DesiredAccuracy = request.PlatformDesiredAccuracy;
            manager.Delegate = listener;

            // we're only listening for a single update
            manager.PausesLocationUpdatesAutomatically = false;

            manager.StartUpdatingLocation();

            var clLocation = await tcs.Task.ConfigureAwait(false);

            if (clLocation == null)
                return null;

            return new Location
            {
                Latitude = clLocation.Coordinate.Latitude,
                Longitude = clLocation.Coordinate.Longitude,
                Accuracy = clLocation.HorizontalAccuracy,
                TimestampUtc = ToDate(clLocation.Timestamp)
            };

            void HandleLocation(CLLocation location)
            {
                manager.StopUpdatingLocation();
                tcs.TrySetResult(location);
            }

            void Cancel()
            {
                manager.StopUpdatingLocation();
                tcs.TrySetResult(null);
            }
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

        class SingleLocationListener : CLLocationManagerDelegate
        {
            bool wasRaised = false;

            public Action<CLLocation> LocationHandler { get; set; }

            public override void LocationsUpdated(CLLocationManager manager, CLLocation[] locations)
            {
                if (wasRaised)
                    return;

                wasRaised = true;

                var location = locations.FirstOrDefault();

                if (location == null)
                    return;

                LocationHandler?.Invoke(location);
            }
        }
    }
}
