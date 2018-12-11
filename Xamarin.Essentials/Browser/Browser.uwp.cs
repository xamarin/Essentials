using System;
using System.Threading.Tasks;

namespace Xamarin.Essentials
{
    public static partial class Browser
    {
        static Task<bool> PlatformOpenAsync(Uri uri, BrowserLaunchMode launchType, BrowserLaunchOptions options) =>
             Windows.System.Launcher.LaunchUriAsync(uri).AsTask();
    }
}
