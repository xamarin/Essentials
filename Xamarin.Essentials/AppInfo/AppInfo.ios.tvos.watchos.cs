using Foundation;
using UIKit;

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

#if __IOS__ || __TVOS__
        static void PlatformShowSettingsUI() =>
            UIApplication.SharedApplication.OpenUrl(new NSUrl(UIApplication.OpenSettingsUrlString));
#else
        static void PlatformShowSettingsUI() =>
            throw new FeatureNotSupportedException();
#endif

#if __IOS__ || __TVOS__
        static AppTheme PlatformRequestedTheme()
        {
            if (!Platform.HasOSVersion(13, 0))
                return AppTheme.Unspecified;

            return Platform.GetCurrentViewController().TraitCollection.UserInterfaceStyle switch
            {
                UIUserInterfaceStyle.Light => AppTheme.Light,
                UIUserInterfaceStyle.Dark => AppTheme.Dark,
                _ => AppTheme.Unspecified
            };
        }
#else
        static AppTheme PlatformRequestedTheme() =>
            AppTheme.Unspecified;
#endif
    }
}
