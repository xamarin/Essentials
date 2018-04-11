using System;
using Windows.Foundation.Metadata;
using Windows.Phone.Devices.Notification;

namespace Xamarin.Essentials
{
    public static partial class Vibration
    {
        internal static bool IsSupported
            => ApiInformation.IsTypePresent("Windows.Phone.Devices.Notification.VibrationDevice") && DefaultDevice != null;

        private static VibrationDevice DefaultDevice => VibrationDevice.GetDefault();

        private static void PlatformVibrate(TimeSpan duration) =>
            DefaultDevice.Vibrate(duration);

        private static void PlatformCancel() =>
            DefaultDevice.Cancel();
    }
}
