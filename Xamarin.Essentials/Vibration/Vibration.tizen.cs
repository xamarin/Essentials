using System;
using System.Linq;
using Tizen.System;

namespace Xamarin.Essentials
{
    public static partial class Vibration
    {
        internal static bool IsSupported
            => Vibrator.NumberOfVibrators > 0;

        static void PlatformVibrate(TimeSpan duration)
            => Vibrator.Vibrators.FirstOrDefault()?.Vibrate((int)duration.TotalMilliseconds, 0);

        static void PlatformCancel()
            => Vibrator.Vibrators.FirstOrDefault()?.Stop();
    }
}
