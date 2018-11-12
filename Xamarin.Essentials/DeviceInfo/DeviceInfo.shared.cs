using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Xamarin.Essentials
{
    public static partial class DeviceInfo
    {
        public static string Model => GetModel();

        public static string Manufacturer => GetManufacturer();

        public static Task<List<StorageInfo>> GetStorageInformationAsync() => PlatformGetStorageInformation();

        public static string Name => GetDeviceName();

        public static string VersionString => GetVersionString();

        public static Version Version => Utils.ParseVersion(VersionString);

        public static string Platform => GetPlatform();

        public static string Idiom => GetIdiom();

        public static DeviceType DeviceType => GetDeviceType();

        public static class Idioms
        {
            // try to match Xamarin.Forms:
            // https://github.com/xamarin/Xamarin.Forms/blob/2.5.1/Xamarin.Forms.Core/TargetIdiom.cs

            public const string Phone = "Phone";
            public const string Tablet = "Tablet";
            public const string Desktop = "Desktop";
            public const string TV = "TV";

            public const string Unsupported = "Unsupported";
        }

        public static class Platforms
        {
            // try to match Xamarin.Forms:
            // https://github.com/xamarin/Xamarin.Forms/blob/2.5.1/Xamarin.Forms.Core/Device.cs#L14-L19

            public const string iOS = "iOS";
            public const string Android = "Android";
            public const string UWP = "UWP";

            public const string Unsupported = "Unsupported";
        }
    }

    public readonly struct StorageInfo : IEquatable<StorageInfo>
    {
        public StorageInfo(ulong capapcity, ulong freeBytes, ulong usedBytes, StorageType type)
        {
            Capacity = capapcity;
            FreeBytes = freeBytes;
            UsedBytes = usedBytes;
            Type = type;
        }

        public ulong Capacity { get; }

        public ulong FreeBytes { get; }

        public ulong UsedBytes { get; }

        public StorageType Type { get; }

        public bool Equals(StorageInfo other) =>
            FreeBytes == other.FreeBytes &&
            Capacity == other.Capacity &&
            UsedBytes == other.UsedBytes &&
            Type == other.Type;

        public static bool operator ==(StorageInfo lhs, StorageInfo rhs) => lhs.Equals(rhs);

        public static bool operator !=(StorageInfo lhs, StorageInfo rhs) => !(lhs == rhs);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            return obj is StorageInfo info && Equals(info);
        }

        public override int GetHashCode() => (Capacity, UsedBytes, FreeBytes).GetHashCode();

        public override string ToString() => $"Capacity: {Capacity}\nFreeBytes: {FreeBytes}\nUsedBytes: {UsedBytes}\nType: {Type}";
    }

    public enum StorageType
    {
        Unknown = 0,
        Internal = 1,
        External = 2
    }

    public enum DeviceType
    {
        Physical,
        Virtual
    }
}
