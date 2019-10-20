using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.Content.PM;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.Content;

namespace Xamarin.Essentials
{
    public partial class Permissions
    {
        public abstract partial class AndroidPermission : BasePermission
        {
            static readonly object locker = new object();
            static int requestCode = 0;

            static Dictionary<string, (int requestCode, TaskCompletionSource<PermissionStatus> tcs)> requests =
                new Dictionary<string, (int, TaskCompletionSource<PermissionStatus>)>();

            public virtual (string androidPermission, bool isRuntime, bool isOptional)[] RequiredPermissions { get; }

            public override Task<PermissionStatus> CheckStatusAsync()
            {
                if (RequiredPermissions == null || RequiredPermissions.Length <= 0)
                    return Task.FromResult(PermissionStatus.Granted);

                var context = Platform.AppContext;
                var targetsMOrHigher = context.ApplicationInfo.TargetSdkVersion >= BuildVersionCodes.M;

                foreach (var p in RequiredPermissions)
                {
                    var ap = p.androidPermission;
                    if (!Permissions.IsDeclaredInManifest(ap))
                        throw new PermissionException($"You need to declare using the permission: `{p.androidPermission}` in your AndroidManifest.xml");

                    var status = PermissionStatus.Denied;

                    if (targetsMOrHigher)
                    {
                        if (ContextCompat.CheckSelfPermission(context, p.androidPermission) != Permission.Granted)
                            status = PermissionStatus.Denied;
                    }
                    else
                    {
                        if (PermissionChecker.CheckSelfPermission(context, p.androidPermission) != PermissionChecker.PermissionGranted)
                            status = PermissionStatus.Denied;
                    }

                    if (status != PermissionStatus.Granted)
                        return Task.FromResult(PermissionStatus.Denied);
                }

                return Task.FromResult(PermissionStatus.Granted);
            }

            public override async Task<PermissionStatus> RequestAsync()
            {
                // Check status before requesting first
                if (await CheckStatusAsync() == PermissionStatus.Granted)
                    return PermissionStatus.Granted;

                TaskCompletionSource<PermissionStatus> tcs;
                var doRequest = true;

                // TODO: Select only optional ones that are in manifest
                var runtimePermissions = RequiredPermissions.Where(p => p.isRuntime).Select(p => p.androidPermission);
                var permissionId = string.Join(';', runtimePermissions);

                lock (locker)
                {
                    if (requests.ContainsKey(permissionId))
                    {
                        tcs = requests[permissionId].tcs;
                        doRequest = false;
                    }
                    else
                    {
                        tcs = new TaskCompletionSource<PermissionStatus>();

                        // Get new request code and wrap it around for next use if it's going to reach max
                        if (++requestCode >= int.MaxValue)
                            requestCode = 1;

                        requests.Add(permissionId, (requestCode, tcs));
                    }
                }

                if (!doRequest)
                    return await tcs.Task;

                if (!MainThread.IsMainThread)
                    throw new PermissionException("Permission request must be invoked on main thread.");

                ActivityCompat.RequestPermissions(Platform.GetCurrentActivity(true), runtimePermissions.ToArray(), requestCode);

                // TODO: should we remove from the dictionary after this?
                return await tcs.Task;
            }

            internal static void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
            {
                lock (locker)
                {
                    // Check our pending requests for one with a matching request code
                    foreach (var kvp in requests)
                    {
                        if (kvp.Value.requestCode == requestCode)
                        {
                            var tcs = kvp.Value.tcs;

                            // Look for any denied requests, and deny the whole request if so
                            // Remember, each PermissionType is tied to 1 or more android permissions
                            // so if any android permissions denied the whole PermissionType is considered denied
                            if (grantResults.Any(g => g == Permission.Denied))
                                tcs.TrySetResult(PermissionStatus.Denied);
                            else
                                tcs.TrySetResult(PermissionStatus.Granted);
                            break;
                        }
                    }
                }
            }
        }

        public sealed class CameraPermission : AndroidPermission
        {
            public override (string androidPermission, bool isRuntime, bool isOptional)[] RequiredPermissions =>
                new (string, bool)[] { (global::Android.Manifest.Permission.Camera, true, false) };
        }
    }
}
