using System;

namespace Xamarin.Essentials
{
    public static partial class DeviceDisplay
    {
        static bool PlatformKeepScreenOn
        {
            get => throw new PlatformNotSupportedException("This API is not currently supported on Tizen.");
            set => throw new PlatformNotSupportedException("This API is not currently supported on Tizen.");
        }

        static DisplayInfo GetMainDisplayInfo()
            => throw new PlatformNotSupportedException("This API is not currently supported on Tizen.");

        static void StartScreenMetricsListeners()
            => throw new PlatformNotSupportedException("This API is not currently supported on Tizen.");

        static void StopScreenMetricsListeners()
            => throw new PlatformNotSupportedException("This API is not currently supported on Tizen.");
    }
}
