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

        static string PlatformGetVersionString()
        {
            var attr = launchingAssembly.GetCustomAttribute<AssemblyVersionAttribute>();
            if (attr != null)
            {
                return attr.Version;
            }

            return string.Empty;
        }

        static string PlatformGetBuild()
        {
            var ver = PlatformGetVersionString();
            if (!string.IsNullOrEmpty(ver))
            {
                var version = new Version(ver);
                return version.Build.ToString();
            }

            return string.Empty;
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
