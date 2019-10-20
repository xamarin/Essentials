using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Xamarin.Essentials
{
    public static partial class Permissions
    {
        //internal static void EnsureDeclared<TPermission>()
        //    where TPermission : BasePermission =>
        //    ;

        //internal static void EnsureDeclared(PermissionType permission) =>
        //    PlatformEnsureDeclared(permission, true);

        //internal static bool EnsureDeclared(PermissionType permission, bool throwIfMissing) =>
        //    PlatformEnsureDeclared(permission, throwIfMissing);

        //internal static Task<PermissionStatus> RequestAsync(PermissionType permission) =>
        //    PlatformRequestAsync(permission);

        public static void ShowSettingsUI() =>
            AppInfo.ShowSettingsUI();

        //internal static async Task RequireAsync(PermissionType permission)
        //{
        //    if (await RequestAsync(permission) != PermissionStatus.Granted)
        //        throw new PermissionException($"{permission} was not granted.");
        //}

        public abstract partial class BasePermission
        {
        }
    }
}
