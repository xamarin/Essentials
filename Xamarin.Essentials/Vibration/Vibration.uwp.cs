using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation.Metadata;
using Windows.Gaming.Input;
using Windows.Phone.Devices.Notification;

namespace Xamarin.Essentials
{
    public static partial class Vibration
    {
        static Gamepad firstGamepad;

        static Gamepad FirstGamepad
        {
            get
            {
                if (firstGamepad == null)
                    firstGamepad = Gamepad.Gamepads.FirstOrDefault();

                return firstGamepad;
            }
        }

        internal static bool IsSupported
            => FirstGamepad != null || (ApiInformation.IsTypePresent("Windows.Phone.Devices.Notification.VibrationDevice") && DefaultDevice != null);

        static VibrationDevice DefaultDevice => VibrationDevice.GetDefault();

        static void PlatformVibrate(TimeSpan duration)
        {
            if (FirstGamepad != null)
            {
                FirstGamepad.Vibration = new GamepadVibration { RightMotor = 0.85 };
                Task.Delay(duration).ContinueWith((t) => FirstGamepad.Vibration = new GamepadVibration { });
            }
            else
            {
                DefaultDevice.Vibrate(duration);
            }
        }

        static void PlatformCancel()
        {
            if (FirstGamepad != null)
            {
                FirstGamepad.Vibration = new GamepadVibration { };
            }
            else
            {
                DefaultDevice.Cancel();
            }
        }
    }
}
