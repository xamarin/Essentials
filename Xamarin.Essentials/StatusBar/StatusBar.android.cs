using System.Drawing;
using Android.OS;

namespace Xamarin.Essentials
{
    public static partial class StatusBar
    {
        internal static void PlatformSetColor(Color color, StatusBarTint tint)
        {
            if (!Platform.HasApiLevel(BuildVersionCodes.Lollipop))
                return;

            var activity = Platform.GetCurrentActivity(true);
            var window = activity.Window;
            window.AddFlags(global::Android.Views.WindowManagerFlags.DrawsSystemBarBackgrounds);
            window.ClearFlags(global::Android.Views.WindowManagerFlags.TranslucentStatus);
            window.SetStatusBarColor(color.ToPlatformColor());

            if (!Platform.HasApiLevel(BuildVersionCodes.M))
                return;

            var flag = (global::Android.Views.StatusBarVisibility)global::Android.Views.SystemUiFlags.LightStatusBar;
            window.DecorView.SystemUiVisibility = tint == StatusBarTint.Light ? flag : 0;
        }
    }
}
