namespace Xamarin.Essentials
{
    public static partial class AppInfo
    {
        static string PlatformGetPackageName() => ThrowHelper.ThrowNotImplementedException<string>();

        static string PlatformGetName() => ThrowHelper.ThrowNotImplementedException<string>();

        static string PlatformGetVersionString() => ThrowHelper.ThrowNotImplementedException<string>();

        static string PlatformGetBuild() => ThrowHelper.ThrowNotImplementedException<string>();

        static void PlatformShowSettingsUI() => ThrowHelper.ThrowNotImplementedException<string>();

        static AppTheme PlatformRequestedTheme() => ThrowHelper.ThrowNotImplementedException<AppTheme>();
    }
}
