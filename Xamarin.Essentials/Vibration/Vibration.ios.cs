using System;
using AudioToolbox;

namespace Xamarin.Essentials
{
    public static partial class Vibration
    {
        internal static bool IsSupported => true;

        private static void PlatformVibrate(TimeSpan duration) =>
            SystemSound.Vibrate.PlaySystemSound();

        private static void PlatformCancel()
        {
        }
    }
}
