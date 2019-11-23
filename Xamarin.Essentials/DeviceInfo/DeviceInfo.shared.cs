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

        public static DevicePlatform Platform => GetPlatform();

        public static DeviceIdiom Idiom => GetIdiom();

        public static DeviceType DeviceType => GetDeviceType();
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

        public override int GetHashCode() => (Capacity, UsedBytes, FreeBytes, Type).GetHashCode();

        public override string ToString() => $"Capacity: {Capacity.ToString()}{Environment.NewLine}FreeBytes: {FreeBytes.ToString()}{Environment.NewLine}UsedBytes: {UsedBytes.ToString()}{Environment.NewLine}Type: {Type}";
    }

    public enum StorageType
    {
        Unknown = 0,
        Internal = 1,
        External = 2
    }

    public enum DeviceType
    {
        Unknown = 0,
        Physical = 1,
        Virtual = 2
    }
}
