﻿using System.Globalization;
using Android.Content;
using Android.Content.PM;
using Android.Content.Res;
using Android.Provider;
#if __ANDROID_29__
using AndroidX.Core.Content.PM;
#else
using Android.Support.V4.Content.PM;
#endif

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
#pragma warning disable CS0618 // Type or member is obsolete
            using (var info = pm.GetPackageInfo(packageName, PackageInfoFlags.MetaData))
#pragma warning restore CS0618 // Type or member is obsolete
            {
                return info.VersionName;
            }
        }

        static string PlatformGetBuild()
        {
            var pm = Platform.AppContext.PackageManager;
            var packageName = Platform.AppContext.PackageName;
#pragma warning disable CS0618 // Type or member is obsolete
            using (var info = pm.GetPackageInfo(packageName, PackageInfoFlags.MetaData))
#pragma warning restore CS0618 // Type or member is obsolete
            {
#if __ANDROID_28__
                return PackageInfoCompat.GetLongVersionCode(info).ToString(CultureInfo.InvariantCulture);
#else
#pragma warning disable CS0618 // Type or member is obsolete
                return info.VersionCode.ToString(CultureInfo.InvariantCulture);
#pragma warning restore CS0618 // Type or member is obsolete
#endif
            }
        }

        static void PlatformShowSettingsUI()
        {
            var context = Platform.GetCurrentActivity(false) ?? Platform.AppContext;

            var settingsIntent = new Intent();
            settingsIntent.SetAction(global::Android.Provider.Settings.ActionApplicationDetailsSettings);
            settingsIntent.AddCategory(Intent.CategoryDefault);
            settingsIntent.SetData(global::Android.Net.Uri.Parse("package:" + PlatformGetPackageName()));

            var flags = ActivityFlags.NewTask | ActivityFlags.NoHistory | ActivityFlags.ExcludeFromRecents;

            settingsIntent.SetFlags(flags);

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
    }
}
