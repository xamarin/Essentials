using System.Threading.Tasks;

namespace Xamarin.Essentials
{
    internal static partial class Permissions
    {
        private static void PlatformEnsureDeclared(PermissionType permission) =>
            throw new NotImplementedInReferenceAssemblyException();

        private static Task<PermissionStatus> PlatformCheckStatusAsync(PermissionType permission) =>
            throw new NotImplementedInReferenceAssemblyException();

        private static Task<PermissionStatus> PlatformRequestAsync(PermissionType permission) =>
            throw new NotImplementedInReferenceAssemblyException();
    }
}
