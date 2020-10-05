using System;

namespace Xamarin.Essentials
{
    public static partial class Vibration
    {
        internal static bool IsSupported
            => ThrowHelper.ThrowNotImplementedException<bool>();

        static void PlatformVibrate(TimeSpan duration)
            => ThrowHelper.ThrowNotImplementedException();

        static void PlatformCancel()
            => ThrowHelper.ThrowNotImplementedException();
    }
}
