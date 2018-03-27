using System;
using Android;
using Android.OS;

namespace Microsoft.Caboodle
{
    public static partial class Vibration
    {
        static bool hasPermission;

        internal static bool IsSupported => true;

        static void PlatformVibrate(TimeSpan duration)
        {
            ValidatePermission();

            var time = (long)duration.TotalMilliseconds;
            if (Platform.HasApiLevel(BuildVersionCodes.O))
            {
                Platform.Vibrator.Vibrate(VibrationEffect.CreateOneShot(time, VibrationEffect.DefaultAmplitude));
            }
            else
            {
#pragma warning disable CS0618 // Type or member is obsolete
                Platform.Vibrator.Vibrate(time);
#pragma warning restore CS0618 // Type or member is obsolete
            }
        }

        static void PlatformCancel()
        {
            ValidatePermission();

            Platform.Vibrator.Cancel();
        }

        static void ValidatePermission()
        {
            if (hasPermission)
                return;

            var permission = Manifest.Permission.Vibrate;
            if (!Platform.HasPermissionInManifest(permission))
                throw new PermissionException(permission);

            hasPermission = true;
        }
    }
}
