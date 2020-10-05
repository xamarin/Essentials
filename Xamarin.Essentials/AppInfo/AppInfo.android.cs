using System;
using System.Globalization;
using Android.Content;
using Android.Content.PM;
using Android.Content.Res;
using Android.Views;

namespace Xamarin.Essentials
{
    public static partial class AppInfo
    {
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
#pragma warning disable CS0618 // Type or member is obsolete
                return info.VersionCode.ToString(CultureInfo.InvariantCulture);
#pragma warning restore CS0618 // Type or member is obsolete
            }
        }

        static void PlatformShowSettingsUI()
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

        static AppTheme PlatformRequestedTheme()
        {
            return (Platform.AppContext.Resources.Configuration.UiMode & UiMode.NightMask) switch
            {
                UiMode.NightYes => AppTheme.Dark,
                UiMode.NightNo => AppTheme.Light,
                _ => AppTheme.Unspecified
            };
        }

        static bool isOverrideActive;

        static BrightnessOverride PlatformSetBrightness(Brightness brightness)
        {
            var window = Platform.GetCurrentActivity(false)?.Window;
            if (window == null)
                return default;

            var attributes = new WindowManagerLayoutParams();
            attributes.CopyFrom(window.Attributes);
            attributes.ScreenBrightness = (float)brightness.Value;
            window.Attributes = attributes;
            isOverrideActive = true;
            return new BrightnessOverride(PlatformGetBrightness(), brightness);
        }

        static Brightness PlatformGetBrightness()
        {
            var currentActivity = Platform.GetCurrentActivity(false);
            return new Brightness(currentActivity?.Window.Attributes.ScreenBrightness ?? -1d);
        }

        internal static void ResetBrightnessOverride() => isOverrideActive = false;

        static bool PlatformIsBrightnessOverrideActive() => isOverrideActive;

        static bool PlatformIsBrightnessSupported() => true;
    }
}
