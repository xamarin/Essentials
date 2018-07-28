using System;
using System.Globalization;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Content.Res;
using Android.Runtime;

namespace Xamarin.Essentials
{
    public static partial class AppInfo
    {
        static AppStateLifecycleListener appState;

        static AppState PlatformState { get; set; }

        static string PlatformGetPackageName() => Platform.AppContext.PackageName;

        static string PlatformGetName()
        {
            var applicationInfo = Platform.AppContext.ApplicationInfo;
            var packageManager = Platform.AppContext.PackageManager;
            return applicationInfo.LoadLabel(packageManager);
        }

        static string PlatformGetVersionString()
        {
            var pm = Platform.AppContext.PackageManager;
            var packageName = Platform.AppContext.PackageName;
            using (var info = pm.GetPackageInfo(packageName, PackageInfoFlags.MetaData))
            {
                return info.VersionName;
            }
        }

        static string PlatformGetBuild()
        {
            var pm = Platform.AppContext.PackageManager;
            var packageName = Platform.AppContext.PackageName;
            using (var info = pm.GetPackageInfo(packageName, PackageInfoFlags.MetaData))
            {
                return info.VersionCode.ToString(CultureInfo.InvariantCulture);
            }
        }

        static void PlatformOpenSettings()
        {
            var context = Platform.GetCurrentActivity(false) ?? Platform.AppContext;

            var settingsIntent = new Intent();
            settingsIntent.SetAction(global::Android.Provider.Settings.ActionApplicationDetailsSettings);
            settingsIntent.AddCategory(Intent.CategoryDefault);
            settingsIntent.SetData(global::Android.Net.Uri.Parse("package:" + PlatformGetPackageName()));
            settingsIntent.AddFlags(ActivityFlags.NewTask);
            settingsIntent.AddFlags(ActivityFlags.NoHistory);
            settingsIntent.AddFlags(ActivityFlags.ExcludeFromRecents);
            context.StartActivity(settingsIntent);
        }

        static void StartStateListeners()
        {
            appState = new AppStateLifecycleListener(UpdateStateCallback);
            var app = Application.Context.ApplicationContext as Application;
            app.RegisterComponentCallbacks(appState);
        }

        static void StopStateListeners()
        {
            var app = Application.Context.ApplicationContext as Application;
            app.UnregisterComponentCallbacks(appState);
            appState.Dispose();
            appState = null;
        }

        internal static void UpdateStateCallback(AppState state)
        {
            PlatformState = state;
            AppInfo.OnStateChanged(PlatformState);
        }
    }

    sealed class AppStateLifecycleListener : Java.Lang.Object, IComponentCallbacks2
    {
        readonly Action<AppState> callback;

        public AppStateLifecycleListener(Action<AppState> callback) => this.callback = callback;

        public void OnConfigurationChanged(Configuration newConfig)
        {
        }

        public void OnLowMemory()
        {
        }

        public void OnTrimMemory([GeneratedEnum] TrimMemory level)
        {
            if (level == TrimMemory.UiHidden)
            {
                callback(AppState.Background);
            }
        }
    }
}
