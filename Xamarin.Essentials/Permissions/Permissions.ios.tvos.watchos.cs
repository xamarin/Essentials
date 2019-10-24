using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using CoreLocation;
using Foundation;

namespace Xamarin.Essentials
{
    public static partial class Permissions
    {
        public static bool IsKeyDeclaredInInfoPlist(string usageKey) =>
            NSBundle.MainBundle.InfoDictionary.ContainsKey(new NSString(usageKey));

        public static TimeSpan LocationTimeout { get; set; } = TimeSpan.FromSeconds(10);

        public abstract class IosBasePermission : BasePermission
        {
            protected virtual Func<IEnumerable<string>> RequiredInfoPlistKeys { get; }

            public override Task<PermissionStatus> CheckStatusAsync()
            {
                EnsureDeclared();

                return Task.FromResult(PermissionStatus.Granted);
            }

            public override async Task<PermissionStatus> RequestAsync()
            {
                var status = await CheckStatusAsync();
                if (status == PermissionStatus.Granted)
                    return PermissionStatus.Granted;

                if (!MainThread.IsMainThread)
                    throw new PermissionException("Permission request must be invoked on main thread.");

                return PermissionStatus.Granted;
            }

            public override void EnsureDeclared()
            {
                var plistKeys = RequiredInfoPlistKeys();

                foreach (var requiredInfoPlistKey in plistKeys)
                {
                    if (!Permissions.IsKeyDeclaredInInfoPlist(requiredInfoPlistKey))
                        throw new PermissionException($"You must set `{requiredInfoPlistKey}` in your Info.plist file to use the Permission: {GetType().Name}.");
                }
            }
        }

        public partial class EventPermissions : IosBasePermission
        {
        }

        public partial class Battery : IosBasePermission
        {
        }

        public partial class CalendarRead : IosBasePermission
        {
        }

        public partial class CalendarWrite : CalendarRead
        {
        }

        public partial class Camera : IosBasePermission
        {
        }

        public partial class ContactsRead : IosBasePermission
        {
        }

        public partial class ContactsWrite : ContactsRead
        {
        }

        public partial class Flashlight : IosBasePermission
        {
        }

        public partial class LaunchApp : IosBasePermission
        {
        }

        public partial class LocationWhenInUse : IosBasePermission
        {
            protected override Func<IEnumerable<string>> RequiredInfoPlistKeys =>
                () => new string[] { "NSLocationWhenInUseUsageDescription" };

            public override Task<PermissionStatus> CheckStatusAsync()
            {
                base.CheckStatusAsync();

                return Task.FromResult(GetLocationStatus(true));
            }

            public override async Task<PermissionStatus> RequestAsync()
            {
                var status = await base.RequestAsync();
                if (status == PermissionStatus.Granted)
                    return status;

                return await RequestLocationAsync(true);
            }

            internal static PermissionStatus GetLocationStatus(bool whenInUse)
            {
                if (!CLLocationManager.LocationServicesEnabled)
                    return PermissionStatus.Disabled;

                var status = CLLocationManager.Status;

                switch (status)
                {
                    case CLAuthorizationStatus.AuthorizedAlways:
                        return PermissionStatus.Granted;
                    case CLAuthorizationStatus.AuthorizedWhenInUse:
                        return whenInUse ? PermissionStatus.Granted : PermissionStatus.Denied;
                    case CLAuthorizationStatus.Denied:
                        return PermissionStatus.Denied;
                    case CLAuthorizationStatus.Restricted:
                        return PermissionStatus.Restricted;
                    default:
                        return PermissionStatus.Unknown;
                }
            }

            internal Task<PermissionStatus> RequestLocationAsync(bool whenInUse)
            {
                var status = GetLocationStatus(whenInUse);
                if (status == PermissionStatus.Granted)
                    return Task.FromResult(status);

                // if (locationManager == null)
                var locationManager = new CLLocationManager();

                var tcs = new TaskCompletionSource<PermissionStatus>(locationManager);

                var previousState = CLLocationManager.Status;

                locationManager.AuthorizationChanged += LocationAuthCallback;

                InitiateLocationRequest(locationManager);

                return tcs.Task;

                void LocationAuthCallback(object sender, CLAuthorizationChangedEventArgs e)
                {
                    if (e.Status == CLAuthorizationStatus.NotDetermined)
                        return;

                    if (previousState == CLAuthorizationStatus.AuthorizedWhenInUse && !whenInUse)
                    {
                        if (e.Status == CLAuthorizationStatus.AuthorizedWhenInUse)
                        {
                            Utils.WithTimeout(tcs.Task, LocationTimeout).ContinueWith(t =>
                            {
                                // Wait for a timeout to see if the check is complete
                                if (!tcs.Task.IsCompleted)
                                {
                                    locationManager.AuthorizationChanged -= LocationAuthCallback;
                                    tcs.TrySetResult(GetLocationStatus(whenInUse));
                                }
                            });
                            return;
                        }
                    }

                    locationManager.AuthorizationChanged -= LocationAuthCallback;

                    tcs.TrySetResult(GetLocationStatus(whenInUse));
                }
            }

            internal virtual void InitiateLocationRequest(CLLocationManager manager)
                => manager.RequestWhenInUseAuthorization();
        }

        public partial class LocationAlways : LocationWhenInUse
        {
        }

        public partial class Maps : IosBasePermission
        {
        }

        public partial class Media : IosBasePermission
        {
        }

        public partial class Microphone : IosBasePermission
        {
        }

        public partial class NetworkState : IosBasePermission
        {
        }

        public partial class Phone : IosBasePermission
        {
        }

        public partial class Photos : IosBasePermission
        {
        }

        public partial class Reminders : IosBasePermission
        {
        }

        public partial class Sensors : IosBasePermission
        {
        }

        public partial class Sms : IosBasePermission
        {
        }

        public partial class StorageRead : IosBasePermission
        {
        }

        public partial class StorageWrite : IosBasePermission
        {
        }

        public partial class Vibrate : IosBasePermission
        {
        }
    }
}
