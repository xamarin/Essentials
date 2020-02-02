using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Xamarin.Essentials
{
    public static partial class Browser
    {
        static Task<bool> PlatformOpenAsync(Uri uri, BrowserLaunchOptions options)
        {
            return new Task<bool>(() =>
            {
                return Process.Start(uri.ToString()) != null;
            });
        }
    }
}
