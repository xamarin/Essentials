using System;
using System.Diagnostics;
using System.Reflection;

namespace Xamarin.Essentials
{
    public static partial class AppInfo
    {
        static readonly Assembly launchingAssembly = Assembly.GetEntryAssembly();

        static string PlatformGetPackageName()
        {
            var attr = launchingAssembly.GetCustomAttribute<AssemblyTitleAttribute>();
            if (attr != null)
            {
                return attr.Title;
            }

            return string.Empty;
        }

        static string PlatformGetName()
        {
            var attr = launchingAssembly.GetCustomAttribute<AssemblyTitleAttribute>();
            if (attr != null)
            {
                return attr.Title;
            }

            return string.Empty;
        }

        static string PlatformGetVersionString() =>
            launchingAssembly.GetName().Version.ToString();

        static string PlatformGetBuild()
        {
            return launchingAssembly.GetName().Version.Build.ToString();
        }

        static void PlatformShowSettingsUI()
        {
            if (Process.Start("ms-settings:appsfeatures-app") is null)
            {
                throw new PlatformNotSupportedException();
            }
        }

        static AppTheme PlatformRequestedTheme() => AppTheme.Unspecified;
    }
}
