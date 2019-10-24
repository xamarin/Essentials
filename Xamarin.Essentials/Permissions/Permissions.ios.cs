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

        public partial class Camera : IosBasePermission
        {
            protected override Func<IEnumerable<string>> RequiredInfoPlistKeys =>
                () => new string[] { "NSCameraUsageDescription" };

            public override async Task<PermissionStatus> CheckStatusAsync()
            {
                await base.CheckStatusAsync();

                return AVPermissions.CheckPermissionsStatus(AVAuthorizationMediaType.Video);
            }

            public override async Task<PermissionStatus> RequestAsync()
            {
                var status = await base.RequestAsync();
                if (status == PermissionStatus.Granted)
                    return status;

                return await AVPermissions.RequestPermissionAsync(AVAuthorizationMediaType.Video);
            }
        }

        public partial class ContactsRead : IosBasePermission
        {
            protected override Func<IEnumerable<string>> RequiredInfoPlistKeys =>
                () => new string[] { "NSContactsUsageDescription" };

            public override async Task<PermissionStatus> CheckStatusAsync()
            {
                await base.CheckStatusAsync();

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

            public override async Task<PermissionStatus> RequestAsync()
            {
                var status = await base.RequestAsync();
                if (status == PermissionStatus.Granted)
                    return status;

                var addressBook = new ABAddressBook();

                var tcs = new TaskCompletionSource<PermissionStatus>();

                addressBook.RequestAccess((success, error) =>
                    tcs.TrySetResult(success ? PermissionStatus.Granted : PermissionStatus.Denied));

                return await tcs.Task;
            }
        }

        public partial class ContactsWrite : ContactsRead
        {
        }

        public partial class Media : IosBasePermission
        {
            protected override Func<IEnumerable<string>> RequiredInfoPlistKeys =>
                () => new string[] { "NSAppleMusicUsageDescription" };

            public override async Task<PermissionStatus> CheckStatusAsync()
            {
                await base.CheckStatusAsync();

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

            public override async Task<PermissionStatus> RequestAsync()
            {
                var status = await base.RequestAsync();
                if (status == PermissionStatus.Granted)
                    return status;

                // Only available in 9.3+
                if (!Platform.HasOSVersion(9, 3))
                    return PermissionStatus.Unknown;

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

                return await tcs.Task;
            }
        }

        public partial class Microphone : IosBasePermission
        {
            protected override Func<IEnumerable<string>> RequiredInfoPlistKeys =>
                () => new string[] { "NSMicrophoneUsageDescription" };

            public override async Task<PermissionStatus> CheckStatusAsync()
            {
                await base.CheckStatusAsync();

                return AVPermissions.CheckPermissionsStatus(AVAuthorizationMediaType.Audio);
            }

            public override async Task<PermissionStatus> RequestAsync()
            {
                var status = await base.RequestAsync();
                if (status == PermissionStatus.Granted)
                    return status;

                return await AVPermissions.RequestPermissionAsync(AVAuthorizationMediaType.Audio);
            }
        }

        public partial class Speech : IosBasePermission
        {
            protected override Func<IEnumerable<string>> RequiredInfoPlistKeys =>
                () => new string[] { "NSSpeechRecognitionUsageDescription" };

            public override async Task<PermissionStatus> CheckStatusAsync()
            {
                await base.CheckStatusAsync();

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

            public override async Task<PermissionStatus> RequestAsync()
            {
                var status = await base.RequestAsync();
                if (status == PermissionStatus.Granted)
                    return status;

                if (!Platform.HasOSVersion(10, 0))
                    return PermissionStatus.Unknown;

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

                return await tcs.Task;
            }
        }
    }
}
