using System;
using Windows.Foundation.Metadata;
using Windows.Phone.Devices.Notification;

namespace Microsoft.Caboodle
{
    public static partial class Vibration
    {
        internal static bool IsSupported
        {
            get
            {
                if (ApiInformation.IsTypePresent("Windows.Phone.Devices.Notification.VibrationDevice"))
                {
                    return VibrationDevice.GetDefault() != null;
                }
                return false;
            }
        }

        static void PlatformVibrate(TimeSpan duration)
        {
            var device = VibrationDevice.GetDefault();
            device.Vibrate(duration);
        }

        static void PlatformCancel()
        {
            var device = VibrationDevice.GetDefault();
            device.Cancel();
        }
    }
}
