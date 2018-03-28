using System;
using System.Globalization;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Services.Store;

namespace Microsoft.Caboodle
{
    public static partial class AppInfo
    {
        static string GetPackageName() => Package.Current.Id.Name;

        static string GetName() => Package.Current.DisplayName;

        static string GetVersionString()
        {
            var version = Package.Current.Id.Version;
            return $"{version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
        }

        static string GetBuild()
            => Package.Current.Id.Version.Build.ToString(CultureInfo.InvariantCulture);

        static async Task<string> PlatformGetLatestVersionStringAsync()
        {
            var context = StoreContext.GetDefault();
            var updates = await context.GetAppAndOptionalStorePackageUpdatesAsync();

            if (updates.Count > 0)
            {
                var version = updates[0].Package.Id.Version;
                return $"{version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
            }
            else
            {
                return VersionString;
            }
        }
    }
}
