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

        public static void OpenSettings() => PlatformOpenSettings();

        static event EventHandler<AppStateChangedEventArgs> AppStateChangedInternal;

        public static AppState State => PlatformState;

        public static event EventHandler<AppStateChangedEventArgs> AppStateChanged
        {
            add
            {
                var wasRunning = AppStateChangedInternal != null;

                AppStateChangedInternal += value;

                if (!wasRunning && AppStateChangedInternal != null)
                    StartStateListeners();
            }

            remove
            {
                var wasRunning = AppStateChangedInternal != null;

                AppStateChangedInternal -= value;

                if (wasRunning && AppStateChangedInternal == null)
                    StopStateListeners();
            }
        }

        internal static void OnStateChanged(AppState state) => OnStateChanged(new AppStateChangedEventArgs(state));

        static void OnStateChanged(AppStateChangedEventArgs e)
            => MainThread.BeginInvokeOnMainThread(() => AppStateChangedInternal?.Invoke(null, e));
    }

    public enum AppState
    {
        Foreground,
        Background
    }

    public class AppStateChangedEventArgs : EventArgs
    {
        internal AppStateChangedEventArgs(AppState appState)
            => State = appState;

        public AppState State { get; }
    }
}
