using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using Windows.ApplicationModel.Contacts;
using Windows.Devices.Enumeration;
using Windows.Devices.Geolocation;

namespace Xamarin.Essentials
{
    public static partial class Permissions
    {
        const string appManifestFilename = "AppxManifest.xml";
        const string appManifestXmlns = "http://schemas.microsoft.com/appx/manifest/foundation/windows10";

        public static bool IsCapabilityDeclared(string capabilityName)
        {
            var doc = XDocument.Load(appManifestFilename, LoadOptions.None);
            var reader = doc.CreateReader();
            var namespaceManager = new XmlNamespaceManager(reader.NameTable);
            namespaceManager.AddNamespace("x", appManifestXmlns);

            // If the manifest doesn't contain a capability we need, throw
            return (!doc.Root.XPathSelectElements($"//x:DeviceCapability[@Name='{capabilityName}']", namespaceManager)?.Any() ?? false) &&
                (!doc.Root.XPathSelectElements($"//x:Capability[@Name='{capabilityName}']", namespaceManager)?.Any() ?? false);
        }

        public abstract partial class UwpPermissionBase : BasePermission
        {
            protected virtual Func<IEnumerable<string>> RequiredDeclarations { get; }

            public override Task<PermissionStatus> CheckStatusAsync()
            {
                EnsureDeclared();
                return Task.FromResult(PermissionStatus.Granted);
            }

            public override Task<PermissionStatus> RequestAsync()
                => CheckStatusAsync();

            public override void EnsureDeclared()
            {
                foreach (var d in RequiredDeclarations())
                {
                    if (!IsCapabilityDeclared(d))
                        throw new PermissionException($"You need to declare the capability `{d}` in your AppxManifest.xml file");
                }
            }
        }

        public partial class Battery : UwpPermissionBase
        {
        }

        public partial class CalendarRead : UwpPermissionBase
        {
        }

        public partial class CalendarWrite : UwpPermissionBase
        {
        }

        public partial class Camera : UwpPermissionBase
        {
        }

        public partial class ContactsRead : UwpPermissionBase
        {
            public override async Task<PermissionStatus> CheckStatusAsync()
            {
                var accessStatus = await ContactManager.RequestStoreAsync(ContactStoreAccessType.AppContactsReadWrite);

                if (accessStatus == null)
                    return PermissionStatus.Denied;

                return PermissionStatus.Granted;
            }
        }

        public partial class ContactsWrite : ContactsRead
        {
        }

        public partial class Flashlight : UwpPermissionBase
        {
        }

        public partial class LaunchApp : UwpPermissionBase
        {
        }

        public partial class LocationWhenInUse : UwpPermissionBase
        {
            protected override Func<IEnumerable<string>> RequiredDeclarations => () =>
                new[] { "location" };

            public override async Task<PermissionStatus> CheckStatusAsync()
            {
                if (!MainThread.IsMainThread)
                    throw new PermissionException("Permission request must be invoked on main thread.");

                var accessStatus = await Geolocator.RequestAccessAsync();
                switch (accessStatus)
                {
                    case GeolocationAccessStatus.Allowed:
                        return PermissionStatus.Granted;
                    case GeolocationAccessStatus.Unspecified:
                        return PermissionStatus.Unknown;
                    default:
                        return PermissionStatus.Denied;
                }
            }
        }

        public partial class LocationAlways : LocationWhenInUse
        {
        }

        public partial class Maps : UwpPermissionBase
        {
        }

        public partial class Media : UwpPermissionBase
        {
        }

        public partial class Microphone : UwpPermissionBase
        {
            protected override Func<IEnumerable<string>> RequiredDeclarations => () =>
                new[] { "microphone" };
        }

        public partial class NetworkState : UwpPermissionBase
        {
        }

        public partial class Phone : UwpPermissionBase
        {
        }

        public partial class Photos : UwpPermissionBase
        {
        }

        public partial class Reminders : UwpPermissionBase
        {
        }

        public partial class Sensors : UwpPermissionBase
        {
            static readonly Guid activitySensorClassId = new Guid("9D9E0118-1807-4F2E-96E4-2CE57142E196");

            public override Task<PermissionStatus> CheckStatusAsync()
            {
                // Determine if the user has allowed access to activity sensors
                var deviceAccessInfo = DeviceAccessInformation.CreateFromDeviceClassId(activitySensorClassId);
                switch (deviceAccessInfo.CurrentStatus)
                {
                    case DeviceAccessStatus.Allowed:
                        return Task.FromResult(PermissionStatus.Granted);
                    case DeviceAccessStatus.DeniedBySystem:
                    case DeviceAccessStatus.DeniedByUser:
                        return Task.FromResult(PermissionStatus.Denied);
                    default:
                        return Task.FromResult(PermissionStatus.Unknown);
                }
            }
        }

        public partial class Sms : UwpPermissionBase
        {
        }

        public partial class Speech : UwpPermissionBase
        {
        }

        public partial class StorageRead : UwpPermissionBase
        {
        }

        public partial class StorageWrite : UwpPermissionBase
        {
        }

        public partial class Vibrate : UwpPermissionBase
        {
        }
    }
}
