using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.Essentials
{
    public static partial class Permissions
    {
        public partial class NetStandardBasePermission : BasePermission
        {
            public override Task<PermissionStatus> CheckStatusAsync() =>
                throw ExceptionUtils.NotSupportedOrImplementedException;

            public override Task<PermissionStatus> RequestAsync() =>
                throw ExceptionUtils.NotSupportedOrImplementedException;

            public override void EnsureDeclared() =>
                throw ExceptionUtils.NotSupportedOrImplementedException;
        }

        public partial class Battery : NetStandardBasePermission
        {
        }

        public partial class CalendarRead : NetStandardBasePermission
        {
        }

        public partial class CalendarWrite : NetStandardBasePermission
        {
        }

        public partial class Camera : NetStandardBasePermission
        {
        }

        public partial class ContactsRead : NetStandardBasePermission
        {
        }

        public partial class ContactsWrite : NetStandardBasePermission
        {
        }

        public partial class Flashlight : NetStandardBasePermission
        {
        }

        public partial class LaunchApp : NetStandardBasePermission
        {
        }

        public partial class LocationWhenInUse : NetStandardBasePermission
        {
        }

        public partial class LocationAlways : NetStandardBasePermission
        {
        }

        public partial class Maps : NetStandardBasePermission
        {
        }

        public partial class Media : NetStandardBasePermission
        {
        }

        public partial class Microphone : NetStandardBasePermission
        {
        }

        public partial class NetworkState : NetStandardBasePermission
        {
        }

        public partial class Phone : NetStandardBasePermission
        {
        }

        public partial class Photos : NetStandardBasePermission
        {
        }

        public partial class Reminders : NetStandardBasePermission
        {
        }

        public partial class Sensors : NetStandardBasePermission
        {
        }

        public partial class Sms : NetStandardBasePermission
        {
        }

        public partial class Speech : NetStandardBasePermission
        {
        }

        public partial class StorageRead : NetStandardBasePermission
        {
        }

        public partial class StorageWrite : NetStandardBasePermission
        {
        }

        public partial class Vibrate : NetStandardBasePermission
        {
        }
    }
}
