using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using AVFoundation;
using Foundation;

namespace Xamarin.Essentials
{
    public partial class Permissions
    {
        public abstract class IosPermission : BasePermission
        {
            protected virtual string[] RequiredInfoPlistKeys { get; }

            public override Task<PermissionStatus> CheckStatusAsync()
            {
                foreach (var requiredInfoPlistKey in RequiredInfoPlistKeys)
                {
                    if (!Permissions.IsKeyDeclaredInInfoPlist(requiredInfoPlistKey))
                        throw new PermissionException($"You must set `{requiredInfoPlistKey}` in your Info.plist file to use the Permission: {GetType().Name}.");
                }

                return Task.FromResult(PermissionStatus.Granted);
            }

            public override Task<PermissionStatus> RequestAsync()
            {
                EnsureMainThread();

                return Task.FromResult(PermissionStatus.Granted);
            }
        }

        public partial class CameraPermission : IosPermission
        {
            protected override string[] RequiredInfoPlistKeys => new string[] { "NSCameraUsageDescription" };

            public override async Task<PermissionStatus> CheckStatusAsync()
            {
                var r = await base.CheckStatusAsync().ConfigureAwait(false);

                if (r != PermissionStatus.Granted)
                    return r;

                return GetAVPermissionStatus(AVMediaType.Video);
            }

            public override Task<PermissionStatus> RequestAsync()
            {
                EnsureMainThread();
                return RequestAVPermissionStatusAsync(AVMediaType.Video);
            }
        }

        public partial class LocationWhileInUsePermission : IosPermission
        {
            protected override string[] RequiredInfoPlistKeys => new string[] { "" };

            public override Task<PermissionStatus> CheckStatusAsync()
            {
                return base.CheckStatusAsync();
            }

            public override Task<PermissionStatus> RequestAsync()
            {
                return base.RequestAsync();
            }
        }
    }
}
