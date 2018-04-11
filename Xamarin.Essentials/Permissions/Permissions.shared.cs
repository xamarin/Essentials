using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Xamarin.Essentials
{
    internal static partial class Permissions
    {
        public static void EnsureDeclared(PermissionType permission) =>
            PlatformEnsureDeclared(permission);

        public static Task<PermissionStatus> CheckStatusAsync(PermissionType permission) =>
            PlatformCheckStatusAsync(permission);

        public static Task<PermissionStatus> RequestAsync(PermissionType permission) =>
            PlatformRequestAsync(permission);

        public static async Task RequireAsync(PermissionType permission)
        {
            if (await RequestAsync(permission) != PermissionStatus.Granted)
                throw new PermissionException($"{permission} was not granted.");
        }
    }
}
