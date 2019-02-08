using System.Threading.Tasks;
using AddressBook;
using CoreLocation;
using Foundation;

namespace Xamarin.Essentials
{
    internal static partial class Permissions
    {
        static void PlatformEnsureDeclared(PermissionType permission)
        {
            var info = NSBundle.MainBundle.InfoDictionary;

            if (permission == PermissionType.LocationWhenInUse)
            {
                if (!info.ContainsKey(new NSString("NSLocationWhenInUseUsageDescription")))
                    throw new PermissionException("You must set `NSLocationWhenInUseUsageDescription` in your Info.plist file to enable Authorization Requests for Location updates.");
            }
        }

        static Task<PermissionStatus> PlatformCheckStatusAsync(PermissionType permission)
        {
            EnsureDeclared(permission);

            switch (permission)
            {
                case PermissionType.Contacts:
                    return Task.FromResult(GetContactsStatus());
                case PermissionType.LocationWhenInUse:
                    return Task.FromResult(GetLocationStatus());
            }

            return Task.FromResult(PermissionStatus.Granted);
        }

        static async Task<PermissionStatus> PlatformRequestAsync(PermissionType permission)
        {
            // Check status before requesting first and only request if Unknown
            var status = await PlatformCheckStatusAsync(permission);
            if (status != PermissionStatus.Unknown)
                return status;

            EnsureDeclared(permission);

            if (!MainThread.IsMainThread)
                throw new PermissionException("Permission request must be invoked on main thread.");

            switch (permission)
            {
                case PermissionType.Contacts:
                    return await RequestContactAsync();
                case PermissionType.LocationWhenInUse:
                    return await RequestLocationAsync();
                default:
                    return PermissionStatus.Granted;
            }
        }

        static PermissionStatus GetContactsStatus()
        {
            var status = ABAddressBook.GetAuthorizationStatus();

            switch (status)
            {
                case ABAuthorizationStatus.Restricted:
                    return PermissionStatus.Restricted;
                case ABAuthorizationStatus.Denied:
                    return PermissionStatus.Denied;
                case ABAuthorizationStatus.Authorized:
                    return PermissionStatus.Granted;
                default:
                    return PermissionStatus.Unknown;
            }
        }

        static PermissionStatus GetLocationStatus()
        {
            if (!CLLocationManager.LocationServicesEnabled)
                return PermissionStatus.Disabled;

            var status = CLLocationManager.Status;

            switch (status)
            {
                case CLAuthorizationStatus.AuthorizedAlways:
                case CLAuthorizationStatus.AuthorizedWhenInUse:
                    return PermissionStatus.Granted;
                case CLAuthorizationStatus.Denied:
                    return PermissionStatus.Denied;
                case CLAuthorizationStatus.Restricted:
                    return PermissionStatus.Restricted;
                default:
                    return PermissionStatus.Unknown;
            }
        }

        static Task<PermissionStatus> RequestContactAsync()
        {
            var status = GetContactsStatus();
            if (status != PermissionStatus.Unknown)
                return Task.FromResult(status);

            var book = new ABAddressBook();

            var tcs = new TaskCompletionSource<PermissionStatus>();

            book.RequestAccess((success, error) =>
            {
                tcs.TrySetResult(success ? PermissionStatus.Granted : PermissionStatus.Denied);
            });

            return tcs.Task;
        }

        static CLLocationManager locationManager;

        static Task<PermissionStatus> RequestLocationAsync()
        {
            locationManager = new CLLocationManager();

            var tcs = new TaskCompletionSource<PermissionStatus>(locationManager);

            locationManager.AuthorizationChanged += LocationAuthCallback;
            locationManager.RequestWhenInUseAuthorization();

            return tcs.Task;

            void LocationAuthCallback(object sender, CLAuthorizationChangedEventArgs e)
            {
                if (e.Status == CLAuthorizationStatus.NotDetermined)
                    return;

                locationManager.AuthorizationChanged -= LocationAuthCallback;
                tcs.TrySetResult(GetLocationStatus());
                locationManager.Dispose();
                locationManager = null;
            }
        }
    }
}
