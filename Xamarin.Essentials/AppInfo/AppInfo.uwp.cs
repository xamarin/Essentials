using System.Globalization;
using Windows.ApplicationModel;
using Windows.UI.ViewManagement;

namespace Xamarin.Essentials
{
    public static partial class AppInfo
    {
        static string PlatformGetPackageName() => Package.Current.Id.Name;

        static string PlatformGetName() => Package.Current.DisplayName;

        static string PlatformGetVersionString()
        {
            var version = Package.Current.Id.Version;
            return $"{version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
        }

        static string PlatformGetBuild() =>
            Package.Current.Id.Version.Build.ToString(CultureInfo.InvariantCulture);

        static void PlatformShowSettingsUI() =>
            Windows.System.Launcher.LaunchUriAsync(new System.Uri("ms-settings:appsfeatures-app")).WatchForError();

        static WindowSize PlatformAppViewInfo()
        {
            var view = ApplicationView.GetForCurrentView();

            return new WindowSize(view.VisibleBounds.Width, view.VisibleBounds.Height);
        }
    }
}
