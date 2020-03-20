using System.Globalization;
using Windows.ApplicationModel;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

namespace Xamarin.Essentials
{
    public static partial class AppInfo
    {
        static UISettings uiSetting;

        static UISettings UISettings => uiSetting ??= new UISettings();

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

        static AppTheme PlatformRequestedTheme() =>
            Application.Current.RequestedTheme == ApplicationTheme.Dark ? AppTheme.Dark : AppTheme.Light;

        static void StartThemeListeners() =>
            UISettings.ColorValuesChanged += UISettingsColorValuesChanged;

        static void StopThemeListeners() =>
            UISettings.ColorValuesChanged -= UISettingsColorValuesChanged;

        static void UISettingsColorValuesChanged(UISettings sender, object args)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                OnAppThemeChanged(PlatformRequestedTheme());
            });
        }
    }
}
