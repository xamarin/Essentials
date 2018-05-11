using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Xamarin.Essentials
{
    internal static partial class Permissions
    {
        static void PlatformEnsureDeclared(PermissionType permission)
        {
            var tizenPrivileges = permission.ToTizenPrivileges();

            // No actual tizen privileges required here, just return
            if (tizenPrivileges == null || !tizenPrivileges.Any())
                return;

            var package = Platform.CurrentPackage;
            foreach (var priv in tizenPrivileges)
            {
                if (!package.Privileges.Contains(priv))
                    throw new PermissionException($"You need to declare the privilege: `{priv}` in your tizen-manifest.xml");
            }
        }

        static Task<PermissionStatus> PlatformCheckStatusAsync(PermissionType permission) =>
            throw new NotImplementedInReferenceAssemblyException();

        static Task<PermissionStatus> PlatformRequestAsync(PermissionType permission) =>
            throw new NotImplementedInReferenceAssemblyException();
    }

    static class PermissionTypeExtensions
    {
        internal static IEnumerable<string> ToTizenPrivileges(this PermissionType permissionType)
        {
            var privileges = new List<string>();

            switch (permissionType)
            {
                case PermissionType.LaunchApp:
                    privileges.Add("http://tizen.org/privilege/appmanager.launch");
                    break;
            }

            return privileges;
        }
    }
}
