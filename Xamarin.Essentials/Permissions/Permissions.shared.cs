using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace Xamarin.Essentials
{
    public static partial class Permissions
    {
        public static Task<PermissionStatus> CheckStatusAsync<TPermission>()
            where TPermission : BasePermission =>
                CreateInstance<TPermission>().CheckStatusAsync();

        public static Task<PermissionStatus> CheckStatusAsync<TPermission>(TPermission permission)
            where TPermission : BasePermission =>
                permission.CheckStatusAsync();

        public static Task<PermissionStatus> RequestAsync<TPermission>()
            where TPermission : BasePermission =>
                CreateInstance<TPermission>().RequestAsync();

        public static Task<PermissionStatus> RequestAsync<TPermission>(TPermission permission)
            where TPermission : BasePermission =>
                permission.RequestAsync();

        internal static void EnsureDeclared<TPermission>()
            where TPermission : BasePermission =>
                CreateInstance<TPermission>().EnsureDeclared();

        internal static void EnsureDeclared<TPermission>(TPermission permission)
            where TPermission : BasePermission =>
                permission.EnsureDeclared();

        internal static TPermission CreateInstance<TPermission>()
            where TPermission : BasePermission =>
#if NETSTANDARD
             throw ExceptionUtils.NotSupportedOrImplementedException;
#else
            (TPermission)System.Runtime.Serialization.FormatterServices.GetUninitializedObject(typeof(TPermission));
#endif

        public static void ShowSettingsUI() =>
            AppInfo.ShowSettingsUI();

        public abstract partial class BasePermission
        {
            [Preserve]
            public BasePermission()
            {
            }

            public abstract Task<PermissionStatus> CheckStatusAsync();

            public abstract Task<PermissionStatus> RequestAsync();

            public abstract void EnsureDeclared();
        }

        public partial class Battery
        {
        }

        public partial class CalendarRead
        {
        }

        public partial class CalendarWrite
        {
        }

        public partial class Camera
        {
        }

        public partial class ContactsRead
        {
        }

        public partial class ContactsWrite
        {
        }

        public partial class Flashlight
        {
        }

        public partial class LaunchApp
        {
        }

        public partial class LocationWhenInUse
        {
        }

        public partial class LocationAlways
        {
        }

        public partial class Maps
        {
        }

        public partial class Media
        {
        }

        public partial class Microphone
        {
        }

        public partial class NetworkState
        {
        }

        public partial class Phone
        {
        }

        public partial class Photos
        {
        }

        public partial class Reminders
        {
        }

        public partial class Sensors
        {
        }

        public partial class Sms
        {
        }

        public partial class Speech
        {
        }

        public partial class StorageRead
        {
        }

        public partial class StorageWrite
        {
        }

        public partial class Vibrate
        {
        }
    }
}
