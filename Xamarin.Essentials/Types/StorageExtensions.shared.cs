using System;
using Xamarin.Essentials;

namespace Xamarin.Essentials.Types
{
    public static class StorageExtensions
    {
        public static byte FreePercentage(this StorageInfo info) =>
            Convert.ToByte(Math.Round(FreePercentage(info.FreeBytes, info.Capacity) * 100, MidpointRounding.AwayFromZero));

        public static byte UsedPercentage(this StorageInfo info) => Convert.ToByte(Math.Round(1 - FreePercentage(info.FreeBytes, info.Capacity)));

        static double FreePercentage(ulong freeBytes, ulong capacity)
        {
            if (capacity == 0)
                return 0;
            var pct = freeBytes / (double)capacity;
            return pct;
        }
    }
}
