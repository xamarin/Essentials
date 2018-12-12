using System;
using System.Threading.Tasks;
using Foundation;
using SafariServices;
using UIKit;

namespace Xamarin.Essentials
{
    public static partial class Browser
    {
        static async Task<bool> PlatformOpenAsync(Uri uri, BrowserLaunchMode launchMode, BrowserLaunchOptions options)
        {
            var nativeUrl = new NSUrl(uri.AbsoluteUri);

            switch (launchMode)
            {
                case BrowserLaunchMode.SystemPreferred:
                    var sfViewController = new SFSafariViewController(nativeUrl, false);
                    var vc = Platform.GetCurrentViewController();

                    if (options.PreferredTitleColor.HasValue)
                        sfViewController.PreferredBarTintColor = (UIColor)options.PreferredTitleColor;

                    if (options.PreferredControlColor.HasValue)
                        sfViewController.PreferredControlTintColor = (UIColor)options.PreferredControlColor;

                    if (sfViewController.PopoverPresentationController != null)
                    {
                        sfViewController.PopoverPresentationController.SourceView = vc.View;
                    }
                    await vc.PresentViewControllerAsync(sfViewController, true);
                    break;
                case BrowserLaunchMode.External:
                    return await UIApplication.SharedApplication.OpenUrlAsync(nativeUrl, new UIApplicationOpenUrlOptions());
            }

            return true;
        }
    }

    public readonly partial struct EssentialsColor
    {
        public static explicit operator UIColor(EssentialsColor x) => new UIColor(x.R, x.G, x.B, x.A);
    }
}
