using System;
using System.Globalization;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Android.Content.PM;

using CaboodlePlatform = Microsoft.Caboodle.Platform;

namespace Microsoft.Caboodle
{
    public static partial class AppInfo
    {
        static string GetPackageName() => CaboodlePlatform.CurrentContext.PackageName;

        static string GetName()
        {
            var applicationInfo = CaboodlePlatform.CurrentContext.ApplicationInfo;
            var packageManager = CaboodlePlatform.CurrentContext.PackageManager;
            return applicationInfo.LoadLabel(packageManager);
        }

        static string GetVersionString()
        {
            var pm = CaboodlePlatform.CurrentContext.PackageManager;
            var packageName = CaboodlePlatform.CurrentContext.PackageName;
            using (var info = pm.GetPackageInfo(packageName, PackageInfoFlags.MetaData))
            {
                return info.VersionName;
            }
        }

        static string GetBuild()
        {
            var pm = CaboodlePlatform.CurrentContext.PackageManager;
            var packageName = CaboodlePlatform.CurrentContext.PackageName;
            using (var info = pm.GetPackageInfo(packageName, PackageInfoFlags.MetaData))
            {
                return info.VersionCode.ToString(CultureInfo.InvariantCulture);
            }
        }

        static async Task<string> PlatformGetLatestVersionStringAsync()
        {
            var version = string.Empty;
            var url = $"https://play.google.com/store/apps/details?id={GetPackageName()}";

            using (var client = new HttpClient())
            {
                var content = await client.GetStringAsync(url);

                var versionMatch = Regex.Match(content, "<div class=\"content\" itemprop=\"softwareVersion\">(.*?)</div>").Groups[1];
                if (versionMatch.Success)
                {
                    version = versionMatch.Value.Trim();
                }
            }

            return version;
        }
    }
}
