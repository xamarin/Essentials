using System;
using System.Threading.Tasks;
using Foundation;
using UIKit;

namespace Xamarin.Essentials
{
    public static partial class Launcher
    {
        public static Task<bool> PlatformCanOpenAsync(string uri)
        {
            return Task.FromResult(UIApplication.SharedApplication.CanOpenUrl(new NSUrl(uri)));
        }

        public static Task<bool> PlatformCanOpenAsync(Uri uri)
        {
            return Task.FromResult(UIApplication.SharedApplication.CanOpenUrl(new NSUrl(uri.ToString())));
        }

        public static async Task PlatformOpenAsync(string uri)
        {
            await UIApplication.SharedApplication.OpenUrlAsync(new NSUrl(uri), new UIApplicationOpenUrlOptions());
        }

        public static async Task PlatformOpenAsync(Uri uri)
        {
            await UIApplication.SharedApplication.OpenUrlAsync(new NSUrl(uri.ToString()), new UIApplicationOpenUrlOptions());
        }
    }
}
