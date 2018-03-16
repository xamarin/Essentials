using System;
using System.Threading.Tasks;
using Foundation;
using SafariServices;

namespace Microsoft.Caboodle
{
    public static partial class Browser
    {
        public static Task OpenAsync(Uri uri, BrowserLaunchingType launchType)
        {
            if (uri == null)
            {
                throw new ArgumentNullException(nameof(uri), "Uri cannot be null.");
            }

            var nativeUrl = new NSUrl(uri.OriginalString);

            switch (launchType)
            {
                case BrowserLaunchingType.SystemBrowser:
                    var sfViewController = new SFSafariViewController(nativeUrl, false);
                    var vc = Platform.GetCurrentViewController();

                    if (sfViewController.PopoverPresentationController != null)
                    {
                        sfViewController.PopoverPresentationController.SourceView = vc.View;
                    }
                    vc.PresentViewController(sfViewController, true, null);
                    break;
                case BrowserLaunchingType.UriLauncher:
                    UIKit.UIApplication.SharedApplication.OpenUrl(nativeUrl);
                    break;
            }

            return Task.CompletedTask;
        }
    }
}
