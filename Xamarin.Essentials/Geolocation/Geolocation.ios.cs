using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CoreLocation;
using Foundation;

namespace Xamarin.Essentials
{
    public static partial class Geolocation
    {
        internal static bool IsSupported
            => CLLocationManager.LocationServicesEnabled;

        static async Task<Location> PlatformLastKnownLocationAsync()
        {
            await Permissions.RequireAsync(PermissionType.LocationWhenInUse);

            var manager = new CLLocationManager();
            var location = manager.Location;

            return location.ToLocation();
        }

        static async Task<Location> PlatformLocationAsync(GeolocationRequest request, CancellationToken cancellationToken)
        {
            await Permissions.RequireAsync(PermissionType.LocationWhenInUse);

            var tcs = new TaskCompletionSource<CLLocation>();

            // the location manager requires an active run loop
            // so just use the main loop
            Platform.BeginInvokeOnMainThread(() =>
            {
                var manager = new CLLocationManager();

                var listener = new SingleLocationListener();
                listener.LocationHandler += HandleLocation;

                cancellationToken = Utils.TimeoutToken(cancellationToken, request.Timeout);
                cancellationToken.Register(Cancel);

                manager.DesiredAccuracy = request.PlatformDesiredAccuracy;
                manager.Delegate = listener;

                // we're only listening for a single update
                manager.PausesLocationUpdatesAutomatically = false;

                manager.StartUpdatingLocation();

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
            });

            // still wait and return on the background thread
            var clLocation = await tcs.Task;

            if (clLocation == null)
                return null;

            return clLocation.ToLocation();
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

                var location = locations.LastOrDefault();

                if (location == null)
                    return;

                LocationHandler?.Invoke(location);
            }
        }
    }
}
