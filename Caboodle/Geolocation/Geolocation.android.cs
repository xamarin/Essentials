using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using AndroidLocation = Android.Locations.Location;

namespace Microsoft.Caboodle
{
    public static partial class Geolocation
    {
        const long twoMinutes = 120000;

        static async Task<Location> PlatformLastKnownLocationAsync()
        {
            await Permissions.RequireAsync(PermissionType.LocationWhenInUse).ConfigureAwait(false);

            var lm = Platform.LocationManager;
            AndroidLocation bestLocation = null;

            foreach (var provider in lm.GetProviders(true))
            {
                var location = lm.GetLastKnownLocation(provider);

                if (bestLocation == null || IsBetterLocation(location, bestLocation))
                    bestLocation = location;
            }

            if (bestLocation == null)
                return null;

            return new Location
            {
                Latitude = bestLocation.Latitude,
                Longitude = bestLocation.Longitude,
                TimestampUtc = bestLocation.GetTimestamp(),
                Accuracy = bestLocation.Accuracy
            };
        }

        static async Task<Location> PlatformLocationAsync(GeolocationRequest request, CancellationToken? cancellationToken)
        {
            await Permissions.RequireAsync(PermissionType.LocationWhenInUse);

            var locationManager = Platform.LocationManager;

            // Get the best possible provider for the requested accuracy
            var provider = GetBestProvider(locationManager, request.DesiredAccuracy);

            // If no providers exist, we can't get a location
            // Let's punt and try to get the last known location
            if (string.IsNullOrEmpty(provider))
                return await GetLastKnownLocationAsync().ConfigureAwait(false);

            var tcs = new TaskCompletionSource<AndroidLocation>();

            var listener = new SingleLocationListener();
            listener.LocationHandler = HandleLocation;

            // Create a new linked cancellation token source if the cancellation token was given
            var cancelTokenSrc = cancellationToken.HasValue ?
                CancellationTokenSource.CreateLinkedTokenSource(cancellationToken.Value) :
                new CancellationTokenSource();

            // If a timeout was given, make the token source cancel after it expires
            if (request.Timeout.HasValue)
                cancelTokenSrc.CancelAfter(request.Timeout.Value);

            // Our Cancel method will handle the actual cancellation logic
            cancelTokenSrc.Token.Register(Cancel);

            // Start getting location updates
            locationManager.RequestLocationUpdates(provider, 0, 0, listener);

            void HandleLocation(AndroidLocation location)
            {
                // Immediately stop location updates
                RemoveUpdates();

                // Set our result
                tcs.TrySetResult(location);
            }

            void Cancel()
            {
                // Try to stop the listener if it's not already
                RemoveUpdates();

                // Set a null result since we cancelled
                tcs.TrySetResult(null);
            }

            void RemoveUpdates()
            {
                try
                {
                    locationManager.RemoveUpdates(listener);
                }
                catch
                {
                }
            }

            var androidLocation = await tcs.Task.ConfigureAwait(false);

            if (androidLocation == null)
                return null;

            return new Location
            {
                Latitude = androidLocation.Latitude,
                Longitude = androidLocation.Longitude,
                TimestampUtc = androidLocation.GetTimestamp(),
                Accuracy = androidLocation.Accuracy
            };
        }

        class SingleLocationListener : Java.Lang.Object, ILocationListener
        {
            bool wasRaised = false;

            public Action<AndroidLocation> LocationHandler { get; set; }

            public void OnLocationChanged(AndroidLocation location)
            {
                if (wasRaised)
                    return;

                wasRaised = true;

                LocationHandler?.Invoke(location);
            }

            public void OnProviderDisabled(string provider)
            {
            }

            public void OnProviderEnabled(string provider)
            {
            }

            public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
            {
            }
        }

        static string GetBestProvider(LocationManager locationManager, GeolocationAccuracy accuracy)
        {
            var criteria = new Criteria();
            criteria.BearingRequired = false;
            criteria.AltitudeRequired = false;
            criteria.SpeedRequired = false;

            switch (accuracy)
            {
                case GeolocationAccuracy.Lowest:
                    criteria.Accuracy = Accuracy.NoRequirement;
                    criteria.HorizontalAccuracy = Accuracy.NoRequirement;
                    criteria.PowerRequirement = Power.NoRequirement;
                    break;
                case GeolocationAccuracy.Low:
                    criteria.Accuracy = Accuracy.Low;
                    criteria.HorizontalAccuracy = Accuracy.Low;
                    criteria.PowerRequirement = Power.Low;
                    break;
                case GeolocationAccuracy.Medium:
                    criteria.Accuracy = Accuracy.Medium;
                    criteria.HorizontalAccuracy = Accuracy.Medium;
                    criteria.PowerRequirement = Power.Medium;
                    break;
                case GeolocationAccuracy.High:
                    criteria.Accuracy = Accuracy.High;
                    criteria.HorizontalAccuracy = Accuracy.High;
                    criteria.PowerRequirement = Power.High;
                    break;
                case GeolocationAccuracy.Best:
                    criteria.Accuracy = Accuracy.Fine;
                    criteria.HorizontalAccuracy = Accuracy.Fine;
                    criteria.PowerRequirement = Power.High;
                    break;
            }

            return locationManager.GetBestProvider(criteria, true)
                ?? locationManager.GetProviders(true).First();
        }

        internal static bool IsBetterLocation(AndroidLocation location, AndroidLocation bestLocation)
        {
            if (bestLocation == null)
                return true;

            var timeDelta = location.Time - bestLocation.Time;

            var isSignificantlyNewer = timeDelta > twoMinutes;
            var isSignificantlyOlder = timeDelta < -twoMinutes;
            var isNewer = timeDelta > 0;

            if (isSignificantlyNewer)
                return true;

            if (isSignificantlyOlder)
                return false;

            var accuracyDelta = (int)(location.Accuracy - bestLocation.Accuracy);
            var isLessAccurate = accuracyDelta > 0;
            var isMoreAccurate = accuracyDelta < 0;
            var isSignificantlyLessAccurage = accuracyDelta > 200;

            var isFromSameProvider = location?.Provider?.Equals(bestLocation?.Provider, StringComparison.OrdinalIgnoreCase) ?? false;

            if (isMoreAccurate)
                return true;

            if (isNewer && !isLessAccurate)
                return true;

            if (isNewer && !isSignificantlyLessAccurage && isFromSameProvider)
                return true;

            return false;
        }
    }
}
