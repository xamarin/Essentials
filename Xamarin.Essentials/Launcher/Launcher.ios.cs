using System;
using System.Threading.Tasks;
using Foundation;
using UIKit;

namespace Xamarin.Essentials.Launcher
{
    public static partial class Launcher
    {
        static Task<bool> PlatformCanOpenAsync(string uri)
        {
            return Task.FromResult(UIApplication.SharedApplication.CanOpenUrl(new NSUrl(uri)));
        }

        static Task<bool> PlatformCanOpenAsync(Uri uri)
        {
            return Task.FromResult(UIApplication.SharedApplication.CanOpenUrl(new NSUrl(uri.ToString())));
        }

        static async Task PlatformOpenAsync(string uri)
        {
            await UIApplication.SharedApplication.OpenUrlAsync(new NSUrl(uri), new UIApplicationOpenUrlOptions());
        }

        static async Task PlatformOpenAsync(Uri uri)
        {
            await UIApplication.SharedApplication.OpenUrlAsync(new NSUrl(uri.ToString()), new UIApplicationOpenUrlOptions());
        }
    }
}
