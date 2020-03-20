using System;

namespace Xamarin.Essentials
{
    public static partial class AppInfo
    {
        public static string PackageName => PlatformGetPackageName();

        public static string Name => PlatformGetName();

        public static string VersionString => PlatformGetVersionString();

        public static Version Version => Utils.ParseVersion(VersionString);

        public static string BuildString => PlatformGetBuild();

        public static void ShowSettingsUI() => PlatformShowSettingsUI();

        public static AppTheme RequestedTheme => PlatformRequestedTheme();

        static AppTheme currentTheme;

        public static event EventHandler<AppThemeChangedEventArgs> RequestedThemeChanged
        {
            add
            {
                var wasRunning = RequestedThemeChangedInternal != null;

                RequestedThemeChangedInternal += value;

                if (!wasRunning && RequestedThemeChangedInternal != null)
                {
                    currentTheme = RequestedTheme;
                    StartThemeListeners();
                }
            }

            remove
            {
                var wasRunning = RequestedThemeChangedInternal != null;

                RequestedThemeChangedInternal -= value;

                if (wasRunning && RequestedThemeChangedInternal == null)
                    StopThemeListeners();
            }
        }

        static event EventHandler<AppThemeChangedEventArgs> RequestedThemeChangedInternal;

        internal static void OnAppThemeChanged(AppTheme appTheme)
        {
            if (currentTheme == appTheme)
                return;

            currentTheme = appTheme;
            OnAppThemeChanged(new AppThemeChangedEventArgs(appTheme));
        }

        static void OnAppThemeChanged(AppThemeChangedEventArgs e) =>
            MainThread.BeginInvokeOnMainThread(() => RequestedThemeChangedInternal?.Invoke(null, e));
    }

    public class AppThemeChangedEventArgs : EventArgs
    {
        public AppThemeChangedEventArgs(AppTheme appTheme) =>
            RequestedTheme = appTheme;

        public AppTheme RequestedTheme { get; }
    }
}
