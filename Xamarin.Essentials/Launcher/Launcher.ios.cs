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
            try
            {
                return Task.FromResult(UIApplication.SharedApplication.CanOpenUrl(new NSUrl(uri)));
            }
            catch(Exception innerException)
            {
                throw new UriFormatException(nameof(uri), innerException);
            }
        }

        public static Task<bool> PlatformCanOpenAsync(Uri uri)
        {
            return PlatformCanOpenAsync(uri.ToString());
        }

        public static async Task PlatformOpenAsync(string uri)
        {
            try
            {
                await UIApplication.SharedApplication.OpenUrlAsync(new NSUrl(uri), new UIApplicationOpenUrlOptions());
            }
            catch(Exception innerException)
            {
                throw new UriFormatException(nameof(uri), innerException);
            }
        }

        public static Task PlatformOpenAsync(Uri uri)
        {
            return PlatformOpenAsync(uri);
        }
    }
}
