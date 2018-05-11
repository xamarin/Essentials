using System.Globalization;
using Tizen.Applications;

namespace Xamarin.Essentials
{
    public static partial class AppInfo
    {
        static string GetPackageName()
            => Application.Current.ApplicationInfo.PackageId;

        static string GetName()
            => Application.Current.ApplicationInfo.Label;

        static string GetVersionString()
            => Platform.CurrentPackage.Version;

        static string GetBuild()
            => Version.Build.ToString(CultureInfo.InvariantCulture);
    }
}
