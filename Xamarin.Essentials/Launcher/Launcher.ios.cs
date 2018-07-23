using System;
using System.Threading.Tasks;
using Foundation;
using UIKit;

namespace Xamarin.Essentials
{
    public static partial class Launcher
    {
        static Task<bool> PlatformCanOpenAsync(Uri uri)
        {
            return Task.FromResult(UIApplication.SharedApplication.CanOpenUrl(new NSUrl(uri.AbsoluteUri)));
        }

        static Task PlatformOpenAsync(Uri uri)
        {
            return UIApplication.SharedApplication.OpenUrlAsync(new NSUrl(uri.AbsoluteUri), new UIApplicationOpenUrlOptions());
        }
    }
}
