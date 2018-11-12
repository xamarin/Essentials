using Xamarin.Essentials;

namespace Xamarin.Essentials.Types
{
    public static class StorageExtensions
    {
        public static double ToKilobytes(this ulong bytes) => bytes / 1024d;

        public static double ToMegabytes(this ulong bytes) => bytes.ToKilobytes() / 1024d;

        public static double ToGigabytes(this ulong bytes) => bytes.ToKilobytes() / 1024d / 1024d;

        public static double FreePercentage(this StorageInfo info) => (double)info.FreeBytes / info.Capacity;

        public static double UsedPercentage(this StorageInfo info) => 1 - info.FreePercentage();
    }
}
