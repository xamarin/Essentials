using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tizen.Security;

namespace Xamarin.Essentials
{
    public static partial class Permissions
    {
        public static bool IsPrivilegeDeclared(string tizenPrivilege)
        {
            var tizenPrivileges = tizenPrivilege;

            if (tizenPrivileges == null || !tizenPrivileges.Any())
                return false;

            var package = Platform.CurrentPackage;

            if (!package.Privileges.Contains(tizenPrivilege))
                return false;

            return true;
        }

        public abstract partial class TizenPermissionBase : BasePermission
        {
            public virtual (string tizenPrivilege, bool isRuntime)[] RequiredPrivileges { get; }

            public override Task<PermissionStatus> CheckStatusAsync()
                => CheckPrivilegeAsync(false);

            public override Task<PermissionStatus> RequestAsync()
                => CheckPrivilegeAsync(true);

            async Task<PermissionStatus> CheckPrivilegeAsync(bool ask)
            {
                if (!RequiredPrivileges.Any())
                    return PermissionStatus.Granted;

                EnsureDeclared();

                var tizenPrivileges = RequiredPrivileges.Where(p => p.isRuntime);

                foreach (var priv in tizenPrivileges)
                {
                    if (PrivacyPrivilegeManager.CheckPermission(priv.tizenPrivilege) == CheckResult.Ask)
                    {
                        if (ask)
                        {
                            var tcs = new TaskCompletionSource<bool>();
                            PrivacyPrivilegeManager.ResponseContext context = null;
                            PrivacyPrivilegeManager.GetResponseContext(priv.tizenPrivilege)
                                .TryGetTarget(out context);
                            void OnResponseFetched(object sender, RequestResponseEventArgs e)
                            {
                                tcs.TrySetResult(e.result == RequestResult.AllowForever);
                            }
                            context.ResponseFetched += OnResponseFetched;
                            PrivacyPrivilegeManager.RequestPermission(priv.tizenPrivilege);
                            var result = await tcs.Task;
                            context.ResponseFetched -= OnResponseFetched;
                            if (result)
                                continue;
                        }
                        return PermissionStatus.Denied;
                    }
                    else if (PrivacyPrivilegeManager.CheckPermission(priv.tizenPrivilege) == CheckResult.Deny)
                    {
                        return PermissionStatus.Denied;
                    }
                }
                return PermissionStatus.Granted;
            }

            public override void EnsureDeclared()
            {
                foreach (var p in RequiredPrivileges)
                {
                    if (!IsPrivilegeDeclared(p.tizenPrivilege))
                        throw new PermissionException($"You need to declare the privilege: `{p.tizenPrivilege}` in your tizen-manifest.xml");
                }
            }
        }

        public partial class Battery : TizenPermissionBase
        {
        }

        public partial class CalendarRead : TizenPermissionBase
        {
        }

        public partial class CalendarWrite : TizenPermissionBase
        {
        }

        public partial class Camera : TizenPermissionBase
        {
            public override (string tizenPrivilege, bool isRuntime)[] RequiredPrivileges =>
                new[] { ("http://tizen.org/privilege/camera", false) };
        }

        public partial class ContactsRead : TizenPermissionBase
        {
        }

        public partial class ContactsWrite : TizenPermissionBase
        {
        }

        public partial class Flashlight : TizenPermissionBase
        {
            public override (string tizenPrivilege, bool isRuntime)[] RequiredPrivileges =>
                new[] { ("http://tizen.org/privilege/led", false) };
        }

        public partial class LaunchApp : TizenPermissionBase
        {
            public override (string tizenPrivilege, bool isRuntime)[] RequiredPrivileges =>
                new[] { ("http://tizen.org/privilege/appmanager.launch", false) };
        }

        public partial class LocationWhenInUse : TizenPermissionBase
        {
            public override (string tizenPrivilege, bool isRuntime)[] RequiredPrivileges =>
                new[] { ("http://tizen.org/privilege/location", true) };
        }

        public partial class LocationAlways : LocationWhenInUse
        {
        }

        public partial class Maps : TizenPermissionBase
        {
            public override (string tizenPrivilege, bool isRuntime)[] RequiredPrivileges =>
                new[]
                {
                    ("http://tizen.org/privilege/internet", false),
                    ("http://tizen.org/privilege/mapservice", false),
                    ("http://tizen.org/privilege/network.get", false)
                };
        }

        public partial class Media : TizenPermissionBase
        {
        }

        public partial class Microphone : TizenPermissionBase
        {
            public override (string tizenPrivilege, bool isRuntime)[] RequiredPrivileges =>
                new[] { ("http://tizen.org/privilege/recorder", false) };
        }

        public partial class NetworkState : TizenPermissionBase
        {
            public override (string tizenPrivilege, bool isRuntime)[] RequiredPrivileges =>
                new[]
                {
                    ("http://tizen.org/privilege/internet", false),
                    ("http://tizen.org/privilege/network.get", false)
                };
        }

        public partial class Phone : TizenPermissionBase
        {
        }

        public partial class Photos : TizenPermissionBase
        {
        }

        public partial class Reminders : TizenPermissionBase
        {
        }

        public partial class Sensors : TizenPermissionBase
        {
        }

        public partial class Sms : TizenPermissionBase
        {
        }

        public partial class Speech : TizenPermissionBase
        {
        }

        public partial class StorageRead : TizenPermissionBase
        {
        }

        public partial class StorageWrite : TizenPermissionBase
        {
        }

        public partial class Vibrate : TizenPermissionBase
        {
            public override (string tizenPrivilege, bool isRuntime)[] RequiredPrivileges =>
                new[] { ("http://tizen.org/privilege/haptic", false) };
        }
    }
}
