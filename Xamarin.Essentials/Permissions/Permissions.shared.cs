using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Xamarin.Essentials
{
#pragma warning disable IDE0040 // Adicionar modificadores de acessibilidade
    internal static partial class Permissions
#pragma warning restore IDE0040 // Adicionar modificadores de acessibilidade
    {
        internal static void EnsureDeclared(PermissionType permission) =>
            PlatformEnsureDeclared(permission);

        internal static Task<PermissionStatus> CheckStatusAsync(PermissionType permission) =>
            PlatformCheckStatusAsync(permission);

        internal static Task<PermissionStatus> RequestAsync(PermissionType permission) =>
            PlatformRequestAsync(permission);

        internal static async Task RequireAsync(PermissionType permission)
        {
            if (await RequestAsync(permission) != PermissionStatus.Granted)
                throw new PermissionException($"{permission} was not granted.");
        }
    }
}
