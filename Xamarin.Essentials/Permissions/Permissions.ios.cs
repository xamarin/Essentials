using System;
using System.Diagnostics;
using System.Threading.Tasks;
using AVFoundation;
using CoreLocation;
using EventKit;
using Foundation;

namespace Xamarin.Essentials
{
    public static partial class Permissions
    {
        public static bool IsKeyDeclaredInInfoPlist(string usageKey) =>
            NSBundle.MainBundle.InfoDictionary.ContainsKey(new NSString(usageKey));

        //static bool PlatformEnsureDeclared(PermissionType permission, bool throwIfMissing)
        //{
        //    bool CheckForInfoPListKey(string key)
        //    {
        //        if (!IsKeyDeclaredInInfoPlist(key))
        //        {
        //            if (throwIfMissing)
        //                throw new PermissionException($"You must set `{key}` in your Info.plist file to use {permission}.");
        //            else
        //                return false;
        //        }

        //        return true;
        //    }

        //    switch (permission)
        //    {
        //        case PermissionType.LocationWhenInUse:
        //            if (!CheckForInfoPListKey("NSLocationWhenInUseUsageDescription"))
        //                return false;
        //            break;
        //        case PermissionType.LocationAlways:
        //            if (!CheckForInfoPListKey("NSLocationAlwaysUsageDescription"))
        //                return false;

        //            // iOS 11+ we should also specify NSLocationAlwaysAndWhenInUseUsageDescription
        //            if (!CheckForInfoPListKey("NSLocationAlwaysAndWhenInUseUsageDescription"))
        //                return false;
        //            break;
        //        case PermissionType.Camera:
        //            if (!CheckForInfoPListKey("NSCameraUsageDescription"))
        //                return false;
        //            break;
        //        case PermissionType.ContactsRead:
        //        case PermissionType.ContactsWrite:
        //            if (!CheckForInfoPListKey("NSContactsUsageDescription"))
        //                return false;
        //            break;
        //        case PermissionType.CalendarRead:
        //        case PermissionType.CalendarWrite:
        //            if (!CheckForInfoPListKey("NSCalendarsUsageDescription"))
        //                return false;
        //            break;
        //        case PermissionType.Microphone:
        //            if (!CheckForInfoPListKey("NSMicrophoneUsageDescription"))
        //                return false;
        //            break;
        //        case PermissionType.Photos:
        //            if (!CheckForInfoPListKey("NSPhotoLibraryUsageDescription"))
        //                return false;
        //            if (!CheckForInfoPListKey("NSPhotoLibraryAddUsageDescription"))
        //                System.Diagnostics.Debug.WriteLine("You may need to set `NSPhotoLibraryAddUsageDescription` in your Info.plist file to use the Photo permission.");
        //            break;
        //        case PermissionType.Reminders:
        //            if (!CheckForInfoPListKey("NSRemindersUsageDescription"))
        //                return false;
        //            break;
        //        case PermissionType.Sensors:
        //            if (!CheckForInfoPListKey("NSMotionUsageDescription"))
        //                return false;
        //            break;
        //        case PermissionType.Speech:
        //            if (!CheckForInfoPListKey("NSSpeechRecognitionUsageDescription"))
        //                return false;
        //            break;
        //    }

        //    return true;
        //}

        // static Task<PermissionStatus> PlatformCheckStatusAsync(PermissionType permission) => throw ExceptionUtils.NotSupportedOrImplementedException;
        // {
        //    EnsureDeclared(permission);
        //    switch (permission)
        //    {
        //        case PermissionType.LocationWhenInUse:
        //            return Task.FromResult(GetLocationStatus(true));
        //        case PermissionType.LocationAlways:
        //            return Task.FromResult(GetLocationStatus(false));
        //        case PermissionType.Camera:
        //            return Task.FromResult(GetAVPermissionStatus(AVMediaType.Video));
        //        case PermissionType.ContactsRead:
        //        case PermissionType.ContactsWrite:
        //            return Task.FromResult(GetContactsPermissionStatus());
        //        case PermissionType.CalendarRead:
        //        case PermissionType.CalendarWrite:
        //            return Task.FromResult(GetEventPermissionStatus(EKEntityType.Event));
        //        case PermissionType.Microphone:
        //            return Task.FromResult(GetAVPermissionStatus(AVMediaType.Audio));
        //        case PermissionType.Photo:
        //            return Task.FromResult(GetPhotosPermissionStatus());
        //        case PermissionType.Reminders:
        //            return Task.FromResult(GetEventPermissionStatus(EKEntityType.Reminder));
        //        case PermissionType.Sensors:
        //            return Task.FromResult(GetSensorsPermissionStatus());
        //    }
        //    return Task.FromResult(PermissionStatus.Granted);
        // }

        //static async Task<PermissionStatus> PlatformRequestAsync(PermissionType permission)
        //{
        //    // Check status before requesting first and only request if Unknown
        //    var status = await PlatformCheckStatusAsync(permission);
        //    if (status != PermissionStatus.Unknown)
        //        return status;

        //    EnsureDeclared(permission);

        //    switch (permission)
        //    {
        //        case PermissionType.LocationWhenInUse:
        //            EnsureMainThread();
        //            return await RequestLocationAsync(true);
        //        case PermissionType.LocationAlways:
        //            EnsureMainThread();
        //            return await RequestLocationAsync(false);
        //        case PermissionType.Camera:
        //            EnsureMainThread();
        //            return await RequestAVPermissionStatusAsync(AVMediaType.Video);
        //        case PermissionType.ContactsRead:
        //        case PermissionType.ContactsWrite:
        //            return await RequestContactsPermission();
        //        case PermissionType.CalendarRead:
        //        case PermissionType.CalendarWrite:
        //            return await RequestEventPermission(EKEntityType.Event);
        //        case PermissionType.Microphone:
        //            return await RequestAVPermissionStatusAsync(AVMediaType.Audio);
        //        case PermissionType.Photos:
        //            return await RequestPhotosPermission();
        //        case PermissionType.Reminders:
        //            return await RequestEventPermission(EKEntityType.Reminder);
        //        default:
        //            return PermissionStatus.Granted;
        //    }
        //}

        internal static void EnsureMainThread()
        {
            if (!MainThread.IsMainThread)
                throw new PermissionException("Permission request must be invoked on main thread.");
        }

        internal static PermissionStatus GetAVPermissionStatus(NSString mediaType)
        {
            var status = AVCaptureDevice.GetAuthorizationStatus(mediaType);
            switch (status)
            {
                case AVAuthorizationStatus.Authorized:
                    return PermissionStatus.Granted;
                case AVAuthorizationStatus.Denied:
                    return PermissionStatus.Denied;
                case AVAuthorizationStatus.Restricted:
                    return PermissionStatus.Restricted;
                default:
                    return PermissionStatus.Unknown;
            }
        }

        internal static async Task<PermissionStatus> RequestAVPermissionStatusAsync(NSString mediaType)
        {
            try
            {
                var auth = await AVCaptureDevice.RequestAccessForMediaTypeAsync(mediaType);
                return auth ? PermissionStatus.Granted : PermissionStatus.Denied;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to get {mediaType} permission: " + ex);
                return PermissionStatus.Unknown;
            }
        }

        #region Location
        static PermissionStatus GetLocationStatus(bool whenInUse)
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

        static CLLocationManager locationManager;

        static Task<PermissionStatus> RequestLocationAsync(bool onlyWhileInUse)
        {
            locationManager = new CLLocationManager();

            var tcs = new TaskCompletionSource<PermissionStatus>(locationManager);

            locationManager.AuthorizationChanged += LocationAuthCallback;
            if (onlyWhileInUse)
                locationManager.RequestWhenInUseAuthorization();
            else
                locationManager.RequestAlwaysAuthorization();

            return tcs.Task;

            void LocationAuthCallback(object sender, CLAuthorizationChangedEventArgs e)
            {
                if (e?.Status == null || e.Status == CLAuthorizationStatus.NotDetermined)
                    return;

                if (locationManager != null)
                    locationManager.AuthorizationChanged -= LocationAuthCallback;

                tcs?.TrySetResult(GetLocationStatus());
                locationManager?.Dispose();
                locationManager = null;
            }
        }
        #endregion

        #region Contacts
        internal static PermissionStatus GetContactsPermissionStatus()
        {
            var status = ABAddressBook.GetAuthorizationStatus();
            switch (status)
            {
                case ABAuthorizationStatus.Authorized:
                    return PermissionStatus.Granted;
                case ABAuthorizationStatus.Denied:
                    return PermissionStatus.Denied;
                case ABAuthorizationStatus.Restricted:
                    return PermissionStatus.Restricted;
                default:
                    return PermissionStatus.Unknown;
            }
        }

        internal static Task<PermissionStatus> RequestContactsPermission()
        {
            if (ContactsPermissionStatus != PermissionStatus.Unknown)
                return Task.FromResult(ContactsPermissionStatus);

            var addressBook = new ABAddressBook();

            var tcs = new TaskCompletionSource<PermissionStatus>();


            addressBook.RequestAccess((success, error) =>
            {
                tcs.TrySetResult((success ? PermissionStatus.Granted : PermissionStatus.Denied));
            });

            return tcs.Task;
        }
        #endregion

        #region Events (Calendar)
        internal static PermissionStatus GetEventPermissionStatus(EKEntityType eventType)
        {
            var status = EKEventStore.GetAuthorizationStatus(eventType);
            switch (status)
            {
                case EKAuthorizationStatus.Authorized:
                    return PermissionStatus.Granted;
                case EKAuthorizationStatus.Denied:
                    return PermissionStatus.Denied;
                case EKAuthorizationStatus.Restricted:
                    return PermissionStatus.Restricted;
                default:
                    return PermissionStatus.Unknown;
            }
        }

        internal static async Task<PermissionStatus> RequestEventPermission(EKEntityType eventType)
        {
            if (GetEventPermissionStatus(eventType) == PermissionStatus.Granted)
                return PermissionStatus.Granted;

            var eventStore = new EKEventStore();

            var results = await eventStore.RequestAccessAsync(eventType);

            return results.Item1 ? PermissionStatus.Granted : PermissionStatus.Denied;
        }
        #endregion

        #region Photos
        internal static PermissionStatus GetPhotosPermissionStatus()
        {
            var status = PHPhotoLibrary.AuthorizationStatus;
            switch (status)
            {
                case PHAuthorizationStatus.Authorized:
                    return PermissionStatus.Granted;
                case PHAuthorizationStatus.Denied:
                    return PermissionStatus.Denied;
                case PHAuthorizationStatus.Restricted:
                    return PermissionStatus.Restricted;
                default:
                    return PermissionStatus.Unknown;
            }
        }

        internal static Task<PermissionStatus> RequestPhotosPermission()
        {
            if (PhotosPermissionStatus != PermissionStatus.Unknown)
                return Task.FromResult(PhotosPermissionStatus);

            var tcs = new TaskCompletionSource<PermissionStatus>();

            PHPhotoLibrary.RequestAuthorization(status =>
            {
                switch (status)
                {
                    case PHAuthorizationStatus.Authorized:
                        tcs.TrySetResult(PermissionStatus.Granted);
                        break;
                    case PHAuthorizationStatus.Denied:
                        tcs.TrySetResult(PermissionStatus.Denied);
                        break;
                    case PHAuthorizationStatus.Restricted:
                        tcs.TrySetResult(PermissionStatus.Restricted);
                        break;
                    default:
                        tcs.TrySetResult(PermissionStatus.Unknown);
                        break;
                }
            });

            return tcs.Task;
        }
        #endregion

        #region Sensors
        internal static PermissionStatus GetSensorsPermissionStatus()
        {
            var sensorStatus = PermissionStatus.Unknown;

            //return disabled if not avaialble.
            if (!CMMotionActivityManager.IsActivityAvailable)
                sensorStatus = PermissionStatus.Disabled;
            else if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
            {
                switch (CMMotionActivityManager.AuthorizationStatus)
                {
                    case CMAuthorizationStatus.Authorized:
                        sensorStatus = PermissionStatus.Granted;
                        break;
                    case CMAuthorizationStatus.Denied:
                        sensorStatus = PermissionStatus.Denied;
                        break;
                    case CMAuthorizationStatus.NotDetermined:
                        sensorStatus = PermissionStatus.Unknown;
                        break;
                    case CMAuthorizationStatus.Restricted:
                        sensorStatus = PermissionStatus.Restricted;
                        break;
                }
            }

            return sensorStatus;
        }

        internal static async Task<PermissionStatus> RequestSensorsPermission()
        {
            if (SensorsPermissionStatus != PermissionStatus.Unknown)
                return SensorsPermissionStatus;

            var activityManager = new CMMotionActivityManager();

            try
            {
                var results = await activityManager.QueryActivityAsync(NSDate.DistantPast, NSDate.DistantFuture, NSOperationQueue.MainQueue);
                if (results != null)
                    return PermissionStatus.Granted;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to query activity manager: " + ex.Message);
                return PermissionStatus.Denied;
            }

            return PermissionStatus.Unknown;
        }
        #endregion

        #region Speech
        internal static Task<PermissionStatus> RequestSpeechPermission()
        {
            if (SpeechPermissionStatus != PermissionStatus.Unknown)
                return Task.FromResult(SpeechPermissionStatus);


            if (!UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            {
                return Task.FromResult(PermissionStatus.Unknown);
            }

            var tcs = new TaskCompletionSource<PermissionStatus>();

            SFSpeechRecognizer.RequestAuthorization(status =>
            {
                switch (status)
                {
                    case SFSpeechRecognizerAuthorizationStatus.Authorized:
                        tcs.TrySetResult(PermissionStatus.Granted);
                        break;
                    case SFSpeechRecognizerAuthorizationStatus.Denied:
                        tcs.TrySetResult(PermissionStatus.Denied);
                        break;
                    case SFSpeechRecognizerAuthorizationStatus.Restricted:
                        tcs.TrySetResult(PermissionStatus.Restricted);
                        break;
                    default:
                        tcs.TrySetResult(PermissionStatus.Unknown);
                        break;
                }
            });
            return tcs.Task;
        }

        internal static PermissionStatus GetSpeechPermissionStatus()
        {
            var status = SFSpeechRecognizer.AuthorizationStatus;
            switch (status)
            {
                case SFSpeechRecognizerAuthorizationStatus.Authorized:
                    return PermissionStatus.Granted;
                case SFSpeechRecognizerAuthorizationStatus.Denied:
                    return PermissionStatus.Denied;
                case SFSpeechRecognizerAuthorizationStatus.Restricted:
                    return PermissionStatus.Restricted;
                default:
                    return PermissionStatus.Unknown;
            }
        }
        #endregion

        #region Media Library	
        internal static PermissionStatus GetMediaLibraryPermissionStatus()
        {
            //Opening settings only open in iOS 9.3+	
            if (!UIDevice.CurrentDevice.CheckSystemVersion(9, 3))
                return PermissionStatus.Unknown;

            var status = MPMediaLibrary.AuthorizationStatus;
            switch (status)
            {
                case MPMediaLibraryAuthorizationStatus.Authorized:
                    return PermissionStatus.Granted;
                case MPMediaLibraryAuthorizationStatus.Denied:
                    return PermissionStatus.Denied;
                case MPMediaLibraryAuthorizationStatus.Restricted:
                    return PermissionStatus.Restricted;
                default:
                    return PermissionStatus.Unknown;
            }
        }

        internal static Task<PermissionStatus> RequestMediaLibraryPermission()
        {

            //Opening settings only open in iOS 9.3+	
            if (!UIDevice.CurrentDevice.CheckSystemVersion(9, 3))
                return Task.FromResult(PermissionStatus.Unknown);

            if (MediaLibraryPermissionStatus != PermissionStatus.Unknown)
                return Task.FromResult(MediaLibraryPermissionStatus);

            var tcs = new TaskCompletionSource<PermissionStatus>();

            MPMediaLibrary.RequestAuthorization(status =>
            {
                switch (status)
                {
                    case MPMediaLibraryAuthorizationStatus.Authorized:
                        tcs.TrySetResult(PermissionStatus.Granted);
                        break;
                    case MPMediaLibraryAuthorizationStatus.Denied:
                        tcs.TrySetResult(PermissionStatus.Denied);
                        break;
                    case MPMediaLibraryAuthorizationStatus.Restricted:
                        tcs.TrySetResult(PermissionStatus.Restricted);
                        break;
                    default:
                        tcs.TrySetResult(PermissionStatus.Unknown);
                        break;
                }
            });

            return tcs.Task;
        }
        #endregion
    }
}
