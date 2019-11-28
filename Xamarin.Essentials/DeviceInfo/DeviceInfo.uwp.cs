using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Security.ExchangeActiveSyncProvisioning;
using Windows.Storage;
using Windows.System.Profile;
using Windows.UI.ViewManagement;

namespace Xamarin.Essentials
{
    public static partial class DeviceInfo
    {
        static readonly EasClientDeviceInformation deviceInfo;
        static readonly string freeSpace = "System.FreeSpace";
        static readonly string capacity = "System.Capacity";
        static readonly string[] properties = { freeSpace, capacity };

        static DeviceInfo()
        {
            deviceInfo = new EasClientDeviceInformation();
        }

        static string GetModel() => deviceInfo.SystemProductName;

        static string GetManufacturer() => deviceInfo.SystemManufacturer;

        static string GetDeviceName() => deviceInfo.FriendlyName;

        static string GetVersionString()
        {
            var version = AnalyticsInfo.VersionInfo.DeviceFamilyVersion;

            if (ulong.TryParse(version, out var v))
            {
                var v1 = (v & 0xFFFF000000000000L) >> 48;
                var v2 = (v & 0x0000FFFF00000000L) >> 32;
                var v3 = (v & 0x00000000FFFF0000L) >> 16;
                var v4 = v & 0x000000000000FFFFL;
                return $"{v1}.{v2}.{v3}.{v4}";
            }

            return version;
        }

        static DevicePlatform GetPlatform() => DevicePlatform.UWP;

        static DeviceIdiom GetIdiom()
        {
            switch (AnalyticsInfo.VersionInfo.DeviceFamily)
            {
                case "Windows.Mobile":
                    return DeviceIdiom.Phone;
                case "Windows.Universal":
                case "Windows.Desktop":
                    {
                        var uiMode = UIViewSettings.GetForCurrentView().UserInteractionMode;
                        return uiMode == UserInteractionMode.Mouse ? DeviceIdiom.Desktop : DeviceIdiom.Tablet;
                    }
                case "Windows.Xbox":
                case "Windows.Team":
                    return DeviceIdiom.TV;
                case "Windows.IoT":
                    return DeviceIdiom.Unknown;
            }

            return DeviceIdiom.Unknown;
        }

        static DeviceType GetDeviceType()
        {
            var isVirtual = deviceInfo.SystemProductName.Contains("Virtual") || deviceInfo.SystemProductName == "HMV domU";

            if (isVirtual)
                return DeviceType.Virtual;

            return DeviceType.Physical;
        }

        static async Task<List<StorageInfo>> PlatformGetStorageInformation()
        {
            var storageInfos = new List<StorageInfo>();
            var folders = await KnownFolders.RemovableDevices.GetFoldersAsync();
            for (var i = 0; i < folders.Count; i++)
            {
                var folder = folders[i];
                var folderProps = await folder.Properties.RetrievePropertiesAsync(properties);
                var fCapacity = (ulong)folderProps[capacity];
                var fFree = (ulong)folderProps[freeSpace];
                storageInfos.Add(new StorageInfo(fCapacity, fFree, fCapacity - fFree, StorageType.External));
            }
            var localProps = await ApplicationData.Current.LocalFolder.Properties.RetrievePropertiesAsync(properties);
            var localCapacity = (ulong)localProps[capacity];
            var localFree = (ulong)localProps[freeSpace];
            storageInfos.Add(new StorageInfo(localCapacity, localFree, localCapacity - localFree, StorageType.Internal));
            return storageInfos;
        }
    }
}
