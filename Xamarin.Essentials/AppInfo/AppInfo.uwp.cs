using System.Globalization;
using Windows.ApplicationModel;

namespace Xamarin.Essentials
{
    public static partial class AppInfo
    {
        private static string GetPackageName() => Package.Current.Id.Name;

        private static string GetName() => Package.Current.DisplayName;

        private static string GetVersionString()
        {
            var version = Package.Current.Id.Version;
            return $"{version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
        }

        private static string GetBuild()
            => Package.Current.Id.Version.Build.ToString(CultureInfo.InvariantCulture);
    }
}
