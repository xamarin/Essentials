using System;
using System.Globalization;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using static Android.App.Application;

namespace Xamarin.Essentials
{
    public static partial class AppInfo
    {
        static readonly Lazy<AppStateLifecycleListener> appState = new Lazy<AppStateLifecycleListener>(
            () => new AppStateLifecycleListener((newState) => UpdateStateCallback(newState)));

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
            var app = Application.Context.ApplicationContext as Application;
            app.RegisterActivityLifecycleCallbacks(appState.Value);
            app.RegisterComponentCallbacks(appState.Value);
        }

        static void StopStateListeners()
        {
            var app = Application.Context.ApplicationContext as Application;
            app.UnregisterActivityLifecycleCallbacks(appState.Value);
            app.UnregisterComponentCallbacks(appState.Value);
        }

        static void UpdateStateCallback(AppState state)
        {
            PlatformState = state;
        }
    }

    sealed class AppStateLifecycleListener : Java.Lang.Object, IActivityLifecycleCallbacks, IComponentCallbacks2
    {
        readonly Action<AppState> callback;

        public AppStateLifecycleListener(Action<AppState> callback)
        {
            this.callback = callback;
        }

        public void OnActivityResumed(Activity activity)
        {
            callback(AppState.Foreground);
            AppInfo.OnStateChanged(AppState.Foreground);
        }

        public void OnTrimMemory([GeneratedEnum] TrimMemory level)
        {
            if (level == TrimMemory.UiHidden)
            {
                callback(AppState.Background);
                AppInfo.OnStateChanged(AppState.Background);
            }
        }

        // Unused from here on
        public void OnActivityCreated(Activity activity, Bundle savedInstanceState)
        {
        }

        public void OnActivityDestroyed(Activity activity)
        {
        }

        public void OnActivityPaused(Activity activity)
        {
        }

        public void OnActivitySaveInstanceState(Activity activity, Bundle outState)
        {
        }

        public void OnActivityStarted(Activity activity)
        {
        }

        public void OnActivityStopped(Activity activity)
        {
        }

        public void OnConfigurationChanged(Configuration newConfig)
        {
        }

        public void OnLowMemory()
        {
        }
    }
}
