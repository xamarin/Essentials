﻿using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
#if __IOS__ || __TVOS__
using UIKit;
#elif __MACOS__
using AppKit;
#endif

namespace Xamarin.Essentials
{
    public static partial class AppInfo
    {
        static string PlatformGetPackageName() => GetBundleValue("CFBundleIdentifier");

        static string PlatformGetName() => GetBundleValue("CFBundleDisplayName") ?? GetBundleValue("CFBundleName");

        static string PlatformGetVersionString() => GetBundleValue("CFBundleShortVersionString");

        static string PlatformGetBuild() => GetBundleValue("CFBundleVersion");

        static string GetBundleValue(string key)
           => NSBundle.MainBundle.ObjectForInfoDictionary(key)?.ToString();

#if __IOS__ || __TVOS__
        static void PlatformShowSettingsUI() =>
            UIApplication.SharedApplication.OpenUrl(new NSUrl(UIApplication.OpenSettingsUrlString));
#elif __MACOS__
        static void PlatformShowSettingsUI()
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                var prefsApp = ScriptingBridge.SBApplication.FromBundleIdentifier("com.apple.systempreferences");
                prefsApp.SendMode = ScriptingBridge.AESendMode.NoReply;
                prefsApp.Activate();
            });
        }
#else
        static void PlatformShowSettingsUI() =>
            throw new FeatureNotSupportedException();
#endif

#if __IOS__ || __TVOS__
        static AppTheme PlatformRequestedTheme()
        {
            if (!Platform.HasOSVersion(13, 0))
                return AppTheme.Unspecified;

            var uiStyle = Platform.GetCurrentUIViewController()?.TraitCollection?.UserInterfaceStyle ??
                UITraitCollection.CurrentTraitCollection.UserInterfaceStyle;

            return uiStyle switch
            {
                UIUserInterfaceStyle.Light => AppTheme.Light,
                UIUserInterfaceStyle.Dark => AppTheme.Dark,
                _ => AppTheme.Unspecified
            };
        }
#elif __MACOS__
        static AppTheme PlatformRequestedTheme()
        {
            if (DeviceInfo.Version >= new Version(10, 14))
            {
                var app = NSAppearance.CurrentAppearance?.FindBestMatch(new string[]
                {
                    NSAppearance.NameAqua,
                    NSAppearance.NameDarkAqua
                });

                if (string.IsNullOrEmpty(app))
                    return AppTheme.Unspecified;

                if (app == NSAppearance.NameDarkAqua)
                    return AppTheme.Dark;
            }
            return AppTheme.Light;
        }
#else
        static AppTheme PlatformRequestedTheme() =>
            AppTheme.Unspecified;
#endif

#if __IOS__

        static AppInfo()
        {
            CoreFoundation.CFNotificationCenter.Darwin.AddObserver("com.apple.springboard.lockcomplete", null, (x, y) => ResetIsActive());
        }

        static bool brightnessOverrideIsActive;

        static BrightnessOverride PlatformSetBrightness(Brightness brightness)
        {
            var oldBrightness = PlatformGetBrightness();
            UIScreen.MainScreen.Brightness = (float)brightness.Value;
            brightnessOverrideIsActive = true;
            return new BrightnessOverride(oldBrightness, brightness);
        }

        static void ResetIsActive() => brightnessOverrideIsActive = false;

        static bool PlatformIsBrightnessOverrideActive() => brightnessOverrideIsActive;

        static Brightness PlatformGetBrightness() => new Brightness(UIScreen.MainScreen.Brightness);

        static bool PlatformIsBrightnessSupported() => true;
#else
        static bool PlatformIsBrightnessSupported() => false;

        static BrightnessOverride PlatformSetBrightness(Brightness brightness) => throw ExceptionUtils.NotSupportedOrImplementedException;

        static Brightness PlatformGetBrightness() => throw ExceptionUtils.NotSupportedOrImplementedException;

        static bool PlatformIsBrightnessOverrideActive() => throw ExceptionUtils.NotSupportedOrImplementedException;
#endif

        internal static bool VerifyHasUrlScheme(string scheme)
        {
            var cleansed = scheme.Replace("://", string.Empty);
            var schemes = GetCFBundleURLSchemes().ToList();
            return schemes.Any(x => x != null && x.Equals(cleansed, StringComparison.InvariantCultureIgnoreCase));
        }

        internal static IEnumerable<string> GetCFBundleURLSchemes()
        {
            var schemes = new List<string>();

            NSObject nsobj = null;
            if (!NSBundle.MainBundle.InfoDictionary.TryGetValue((NSString)"CFBundleURLTypes", out nsobj))
                return schemes;

            var array = nsobj as NSArray;

            if (array == null)
                return schemes;

            for (nuint i = 0; i < array.Count; i++)
            {
                var d = array.GetItem<NSDictionary>(i);
                if (d == null || !d.Any())
                    continue;

                if (!d.TryGetValue((NSString)"CFBundleURLSchemes", out nsobj))
                    continue;

                var a = nsobj as NSArray;
                var urls = ConvertToIEnumerable<NSString>(a).Select(x => x.ToString()).ToArray();
                foreach (var url in urls)
                    schemes.Add(url);
            }

            return schemes;
        }

        static IEnumerable<T> ConvertToIEnumerable<T>(NSArray array)
            where T : class, ObjCRuntime.INativeObject
        {
            for (nuint i = 0; i < array.Count; i++)
                yield return array.GetItem<T>(i);
        }
    }
}
