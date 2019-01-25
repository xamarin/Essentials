using System;
using System.Threading.Tasks;
using Foundation;
using SafariServices;
using UIKit;

namespace Xamarin.Essentials
{
    public static partial class Browser
    {
        static async Task<bool> PlatformOpenAsync(Uri uri, BrowserLaunchOptions options)
        {
            var nativeUrl = new NSUrl(uri.AbsoluteUri);

            switch (options.LaunchMode)
            {
                case BrowserLaunchMode.SystemPreferred:
                    var sfViewController = new SFSafariViewController(nativeUrl, false);
                    var vc = Platform.GetCurrentViewController();

                    if (options.PreferredTitleColor.HasValue)
                        sfViewController.PreferredBarTintColor = options.PreferredTitleColor.Value.ToPlatformColor();

                    if (options.PrefferedControlColor.HasValue)
                        sfViewController.PreferredControlTintColor = options.PrefferedControlColor.Value.ToPlatformColor();

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
}
