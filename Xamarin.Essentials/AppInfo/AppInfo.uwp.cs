using System.Globalization;
using Windows.ApplicationModel;
using Windows.UI.Core;
using Windows.UI.Xaml;

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

        static void PlatformOpenSettings() =>
            Windows.System.Launcher.LaunchUriAsync(new System.Uri("ms-settings:appsfeatures-app")).WatchForError();

        static AppState PlatformState => Window.Current.Visible ? AppState.Foreground : AppState.Background;

        static void StartStateListeners() => Window.Current.VisibilityChanged += VisibilityChanged;

        static void VisibilityChanged(object sender, VisibilityChangedEventArgs e)
        {
            var state = e.Visible ? AppState.Foreground : AppState.Background;
            MainThread.BeginInvokeOnMainThread(() => OnStateChanged(state));
            e.Handled = true;
        }

        static void StopStateListeners() => Window.Current.VisibilityChanged -= VisibilityChanged;
    }
}
