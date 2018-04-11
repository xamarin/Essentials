using Foundation;

namespace Xamarin.Essentials
{
    public static partial class AppInfo
    {
        private static string GetPackageName() => GetBundleValue("CFBundleIdentifier");

        private static string GetName() => GetBundleValue("CFBundleDisplayName") ?? GetBundleValue("CFBundleName");

        private static string GetVersionString() => GetBundleValue("CFBundleShortVersionString");

        private static string GetBuild() => GetBundleValue("CFBundleVersion");

        private static string GetBundleValue(string key)
           => NSBundle.MainBundle.ObjectForInfoDictionary(key).ToString();
    }
}
