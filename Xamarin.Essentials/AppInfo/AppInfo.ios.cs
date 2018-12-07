using Foundation;
using UIKit;

namespace Xamarin.Essentials
{
    public static partial class AppInfo
    {
        static string PlatformGetPackageName() => GetBundleValue("CFBundleIdentifier");

        static string PlatformGetName() => GetBundleValue("CFBundleDisplayName") ?? GetBundleValue("CFBundleName");

        static string PlatformGetVersionString() => GetBundleValue("CFBundleVersion");

        static string PlatformGetBuild() => GetBundleValue("CFBundleShortVersionString");

        static string GetBundleValue(string key)
           => NSBundle.MainBundle.ObjectForInfoDictionary(key)?.ToString();

        static void PlatformShowSettingsUI() =>
            UIApplication.SharedApplication.OpenUrl(new NSUrl(UIApplication.OpenSettingsUrlString));
    }
}
