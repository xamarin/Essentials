using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android;
using Android.Content.PM;
using Android.Support.V4.App;
using Android.Support.V4.Content;

namespace Microsoft.Caboodle
{
    internal static partial class Permissions
    {
        static TaskCompletionSource<PermissionStatus> tcs;

        static Task PlatformEnsureDeclaredAsync(PermissionType permission)
        {
            var androidPermissions = permission.ToAndroidPermissions();

            if (androidPermissions == null || !androidPermissions.Any())
                return Task.CompletedTask;

            var context = Platform.CurrentContext;

            foreach (var ap in androidPermissions)
            {
                var packageInfo = context.PackageManager.GetPackageInfo(context.PackageName, PackageInfoFlags.Permissions);
                var requestedPermissions = packageInfo?.RequestedPermissions;

                if (!requestedPermissions?.Any(r => r.Equals(ap, StringComparison.OrdinalIgnoreCase)) ?? false)
                    throw new PermissionException($"You need to declare the permission: `{ap}` in your AndroidManifest.xml");
            }

            return Task.CompletedTask;
        }

        static Task<PermissionStatus> PlatformCheckStatusAsync(PermissionType permission)
        {
            PlatformEnsureDeclaredAsync(permission);

            var androidPermissions = permission.ToAndroidPermissions();

            if (androidPermissions == null || !androidPermissions.Any())
                return Task.FromResult(PermissionStatus.Unknown);

            var hasApiM = Platform.HasApiLevel(Android.OS.BuildVersionCodes.M);
            var context = Platform.CurrentContext;

            foreach (var ap in androidPermissions)
            {
                if (hasApiM)
                {
                    if (context.CheckSelfPermission(ap) != Permission.Granted)
                        return Task.FromResult(PermissionStatus.Denied);
                }
                else
                {
                    if (PermissionChecker.CheckSelfPermission(context, ap) != PermissionChecker.PermissionGranted)
                        return Task.FromResult(PermissionStatus.Denied);
                }
            }

            return Task.FromResult(PermissionStatus.Granted);
        }

        static async Task<PermissionStatus> PlatformRequestAsync(PermissionType permission)
        {
            if (await PlatformCheckStatusAsync(permission) == PermissionStatus.Granted)
                return PermissionStatus.Granted;

            var androidPermissions = permission.ToAndroidPermissions();

            var activity = Platform.CurrentActivity;

            tcs = new TaskCompletionSource<PermissionStatus>();

            ActivityCompat.RequestPermissions(activity, androidPermissions.ToArray(), 24600913);

            var result = await tcs.Task;

            return result;
        }
    }

    internal static class PermissionTypeExtensions
    {
        internal static string[] ToAndroidPermissions(this PermissionType permissionType)
        {
            switch (permissionType)
            {
                case PermissionType.Battery:
                    return new[] { Manifest.Permission.BatteryStats };
                case PermissionType.LocationWhenInUse:
                    return new[] { Manifest.Permission.AccessFineLocation, Manifest.Permission.AccessCoarseLocation };
                case PermissionType.NetworkState:
                    return new[] { Manifest.Permission.AccessNetworkState };
            }

            return null;
        }
    }
}
