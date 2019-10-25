using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using AddressBook;
using AVFoundation;
using MediaPlayer;
using Speech;

namespace Xamarin.Essentials
{
    public static partial class Permissions
    {
        internal static partial class AVPermissions
        {
            internal static PermissionStatus CheckPermissionsStatus(AVAuthorizationMediaType mediaType)
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

            internal static async Task<PermissionStatus> RequestPermissionAsync(AVAuthorizationMediaType mediaType)
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
        }

        public partial class Camera : BasePlatformPermission
        {
            protected override Func<IEnumerable<string>> RequiredInfoPlistKeys =>
                () => new string[] { "NSCameraUsageDescription" };

            public override Task<PermissionStatus> CheckStatusAsync()
            {
                EnsureDeclared();

                return Task.FromResult(AVPermissions.CheckPermissionsStatus(AVAuthorizationMediaType.Video));
            }

            public override async Task<PermissionStatus> RequestAsync()
            {
                EnsureDeclared();

                var status = AVPermissions.CheckPermissionsStatus(AVAuthorizationMediaType.Video);
                if (status == PermissionStatus.Granted)
                    return status;

                EnsureMainThread();

                return await AVPermissions.RequestPermissionAsync(AVAuthorizationMediaType.Video);
            }
        }

        public partial class ContactsRead : BasePlatformPermission
        {
            protected override Func<IEnumerable<string>> RequiredInfoPlistKeys =>
                () => new string[] { "NSContactsUsageDescription" };

            public override Task<PermissionStatus> CheckStatusAsync()
            {
                EnsureDeclared();

                return Task.FromResult(GetAddressBookPermissionStatus());
            }

            public override Task<PermissionStatus> RequestAsync()
            {
                EnsureDeclared();

                var status = GetAddressBookPermissionStatus();
                if (status == PermissionStatus.Granted)
                    return Task.FromResult(status);

                EnsureMainThread();

                return RequestAddressBookPermission();
            }

            internal static PermissionStatus GetAddressBookPermissionStatus()
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

            internal static Task<PermissionStatus> RequestAddressBookPermission()
            {
                var addressBook = new ABAddressBook();

                var tcs = new TaskCompletionSource<PermissionStatus>();

                addressBook.RequestAccess((success, error) =>
                    tcs.TrySetResult(success ? PermissionStatus.Granted : PermissionStatus.Denied));

                return tcs.Task;
            }
        }

        public partial class ContactsWrite : BasePlatformPermission
        {
            protected override Func<IEnumerable<string>> RequiredInfoPlistKeys =>
                () => new string[] { "NSContactsUsageDescription" };

            public override Task<PermissionStatus> CheckStatusAsync()
            {
                EnsureDeclared();

                return Task.FromResult(ContactsRead.GetAddressBookPermissionStatus());
            }

            public override Task<PermissionStatus> RequestAsync()
            {
                EnsureDeclared();

                var status = ContactsRead.GetAddressBookPermissionStatus();
                if (status == PermissionStatus.Granted)
                    return Task.FromResult(status);

                EnsureMainThread();

                return ContactsRead.RequestAddressBookPermission();
            }
        }

        public partial class Media : BasePlatformPermission
        {
            protected override Func<IEnumerable<string>> RequiredInfoPlistKeys =>
                () => new string[] { "NSAppleMusicUsageDescription" };

            public override Task<PermissionStatus> CheckStatusAsync()
            {
                EnsureDeclared();

                return Task.FromResult(GetMediaPermissionStatus());
            }

            public override Task<PermissionStatus> RequestAsync()
            {
                EnsureDeclared();

                var status = GetMediaPermissionStatus();
                if (status == PermissionStatus.Granted)
                    return Task.FromResult(status);

                EnsureMainThread();

                return RequestMediaPermission();
            }

            internal static PermissionStatus GetMediaPermissionStatus()
            {
                // Only available in 9.3+
                if (!Platform.HasOSVersion(9, 3))
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

            internal static Task<PermissionStatus> RequestMediaPermission()
            {
                // Only available in 9.3+
                if (!Platform.HasOSVersion(9, 3))
                    return Task.FromResult(PermissionStatus.Unknown);

                var tcs = new TaskCompletionSource<PermissionStatus>();

                MPMediaLibrary.RequestAuthorization(s =>
                {
                    switch (s)
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
        }

        public partial class Microphone : BasePlatformPermission
        {
            protected override Func<IEnumerable<string>> RequiredInfoPlistKeys =>
                () => new string[] { "NSMicrophoneUsageDescription" };

            public override Task<PermissionStatus> CheckStatusAsync()
            {
                EnsureDeclared();

                return Task.FromResult(AVPermissions.CheckPermissionsStatus(AVAuthorizationMediaType.Audio));
            }

            public override Task<PermissionStatus> RequestAsync()
            {
                EnsureDeclared();

                var status = AVPermissions.CheckPermissionsStatus(AVAuthorizationMediaType.Audio);
                if (status == PermissionStatus.Granted)
                    return Task.FromResult(status);

                EnsureMainThread();

                return AVPermissions.RequestPermissionAsync(AVAuthorizationMediaType.Audio);
            }
        }

        public partial class Speech : BasePlatformPermission
        {
            protected override Func<IEnumerable<string>> RequiredInfoPlistKeys =>
                () => new string[] { "NSSpeechRecognitionUsageDescription" };

            public override Task<PermissionStatus> CheckStatusAsync()
            {
                EnsureDeclared();

                return Task.FromResult(GetSpeechPermissionStatus());
            }

            public override Task<PermissionStatus> RequestAsync()
            {
                EnsureDeclared();

                var status = GetSpeechPermissionStatus();
                if (status == PermissionStatus.Granted)
                    return Task.FromResult(status);

                EnsureMainThread();

                return RequestSpeechPermission();
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

            internal static Task<PermissionStatus> RequestSpeechPermission()
            {
                if (!Platform.HasOSVersion(10, 0))
                    return Task.FromResult(PermissionStatus.Unknown);

                var tcs = new TaskCompletionSource<PermissionStatus>();

                SFSpeechRecognizer.RequestAuthorization(s =>
                {
                    switch (s)
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
        }
    }
}
