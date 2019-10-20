using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android;
using Android.Content.PM;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.Content;

namespace Xamarin.Essentials
{
    public static partial class Permissions
    {
        public static bool IsDeclaredInManifest(string permission)
        {
            var context = Platform.AppContext;
            var packageInfo = context.PackageManager.GetPackageInfo(context.PackageName, PackageInfoFlags.Permissions);
            var requestedPermissions = packageInfo?.RequestedPermissions;

            return requestedPermissions?.Any(r => r.Equals(permission, StringComparison.OrdinalIgnoreCase)) ?? false;
        }

        internal static void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
            => AndroidPermission.OnRequestPermissionsResult(requestCode, permissions, grantResults);
    }

    static class PermissionTypeExtensions
    {
        internal static IEnumerable<string> ToAndroidPermissions(this PermissionType permissionType, bool onlyRuntimePermissions)
        {
            var permissions = new List<(string permission, bool runtimePermission)>();

            switch (permissionType)
            {
                case PermissionType.Battery:
                    permissions.Add((Manifest.Permission.BatteryStats, false));
                    break;
                case PermissionType.Camera:
                    permissions.Add((Manifest.Permission.Camera, true));
                    break;
                case PermissionType.Flashlight:
                    permissions.Add((Manifest.Permission.Camera, true));
                    permissions.Add((Manifest.Permission.Flashlight, false));
                    break;
                case PermissionType.LocationWhenInUse:
                    permissions.Add((Manifest.Permission.AccessFineLocation, true));
                    permissions.Add((Manifest.Permission.AccessCoarseLocation, true));
                    break;
                case PermissionType.LocationAlways:
#if __ANDROID_100__
                    permissions.Add((Manifest.Permission.AccessBackgroundLocation, true));
#endif
                    permissions.Add((Manifest.Permission.AccessFineLocation, true));
                    permissions.Add((Manifest.Permission.AccessCoarseLocation, true));
                    break;
                case PermissionType.NetworkState:
                    permissions.Add((Manifest.Permission.AccessNetworkState, false));
                    break;
                case PermissionType.Vibrate:
                    permissions.Add((Manifest.Permission.Vibrate, false));
                    break;
                case PermissionType.WriteExternalStorage:
                case PermissionType.StorageWrite:
                    permissions.Add((Manifest.Permission.WriteExternalStorage, true));
                    break;
                case PermissionType.StorageRead:
                    permissions.Add((Manifest.Permission.ReadExternalStorage, true));
                    break;
                case PermissionType.CalendarRead:
                    permissions.Add((Manifest.Permission.ReadCalendar, true));
                    break;
                case PermissionType.CalendarWrite:
                    permissions.Add((Manifest.Permission.WriteCalendar, true));
                    break;
                case PermissionType.ContactsRead:
                    permissions.Add((Manifest.Permission.ReadContacts, true));
                    break;
                case PermissionType.ContactsWrite:
                    permissions.Add((Manifest.Permission.WriteContacts, true));
                    break;
                case PermissionType.Microphone:
                    permissions.Add((Manifest.Permission.RecordAudio, true));
                    break;
                case PermissionType.Phone:
                    permissions.Add((Manifest.Permission.ReadPhoneState, true));
                    if (Permissions.IsDeclaredInManifest(Manifest.Permission.CallPhone))
                        permissions.Add((Manifest.Permission.CallPhone, true));
                    if (Permissions.IsDeclaredInManifest(Manifest.Permission.ReadCallLog))
                        permissions.Add((Manifest.Permission.ReadCallLog, true));
                    if (Permissions.IsDeclaredInManifest(Manifest.Permission.WriteCallLog))
                        permissions.Add((Manifest.Permission.WriteCallLog, true));
                    if (Permissions.IsDeclaredInManifest(Manifest.Permission.AddVoicemail))
                        permissions.Add((Manifest.Permission.AddVoicemail, true));
                    if (Permissions.IsDeclaredInManifest(Manifest.Permission.UseSip))
                        permissions.Add((Manifest.Permission.UseSip, true));
                    if (Permissions.IsDeclaredInManifest(Manifest.Permission.ProcessOutgoingCalls))
                        permissions.Add((Manifest.Permission.ProcessOutgoingCalls, true));
                    break;
                case PermissionType.Sensors:
                    permissions.Add((Manifest.Permission.BodySensors, true));
                    break;
                case PermissionType.Sms:
                    permissions.Add((Manifest.Permission.ReceiveSms, true));
                    if (Permissions.IsDeclaredInManifest(Manifest.Permission.SendSms))
                        permissions.Add((Manifest.Permission.SendSms, true));
                    if (Permissions.IsDeclaredInManifest(Manifest.Permission.ReadSms))
                        permissions.Add((Manifest.Permission.ReadSms, true));
                    if (Permissions.IsDeclaredInManifest(Manifest.Permission.ReceiveWapPush))
                        permissions.Add((Manifest.Permission.ReceiveWapPush, true));
                    if (Permissions.IsDeclaredInManifest(Manifest.Permission.ReceiveMms))
                        permissions.Add((Manifest.Permission.ReceiveMms, true));
                    break;
            }

            if (onlyRuntimePermissions)
            {
                return permissions
                    .Where(p => p.runtimePermission)
                    .Select(p => p.permission);
            }

            return permissions.Select(p => p.permission);
        }
    }
}
