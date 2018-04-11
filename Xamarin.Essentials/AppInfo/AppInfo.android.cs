using System.Globalization;
using Android.Content.PM;

namespace Xamarin.Essentials
{
    public static partial class AppInfo
    {
        private static string GetPackageName() => Platform.CurrentContext.PackageName;

        private static string GetName()
        {
            var applicationInfo = Platform.CurrentContext.ApplicationInfo;
            var packageManager = Platform.CurrentContext.PackageManager;
            return applicationInfo.LoadLabel(packageManager);
        }

        private static string GetVersionString()
        {
            var pm = Platform.CurrentContext.PackageManager;
            var packageName = Platform.CurrentContext.PackageName;
            using (var info = pm.GetPackageInfo(packageName, PackageInfoFlags.MetaData))
            {
                return info.VersionName;
            }
        }

        private static string GetBuild()
        {
            var pm = Platform.CurrentContext.PackageManager;
            var packageName = Platform.CurrentContext.PackageName;
            using (var info = pm.GetPackageInfo(packageName, PackageInfoFlags.MetaData))
            {
                return info.VersionCode.ToString(CultureInfo.InvariantCulture);
            }
        }
    }
}
