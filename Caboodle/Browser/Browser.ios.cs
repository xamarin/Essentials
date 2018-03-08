using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Caboodle
{
    public partial class Browser
    {
        public static async Task OpenAsync(string uri)
        {
            if (string.IsNullOrEmpty(uri))
            {
                throw new ArgumentNullException($"Uri cannot be null or empty.");
            }

            await Browser.OpenAsync(new System.Uri(uri));

            return;
        }

        public static async Task OpenAsync(System.Uri uri)
        {
            await Task.Run(
                () =>
                {
                    if (uri == null)
                    {
                        throw new ArgumentNullException($"Uri cannot be null.");
                    }

                    var url_native = new Foundation.NSUrl(uri.OriginalString);

                    if (AlwaysUseExternal)
                    {
                        System.Diagnostics.Debug.WriteLine("External");

                        Platform.BeginInvokeOnMainThread(
                                () =>
                                UIKit.UIApplication.SharedApplication.OpenUrl(url_native));
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("Safari");
                        var sfViewController = new SafariServices.SFSafariViewController(url_native, false);

                        // set colors here, we can set tintcolors on iOS 10+
                        Platform.BeginInvokeOnMainThread(
                                () =>
                                {
                                    var vc = GetVisibleViewController();

                                    if (sfViewController.PopoverPresentationController != null)
                                    {
                                        sfViewController.PopoverPresentationController.SourceView = vc.View;
                                    }
                                    vc.PresentViewController(sfViewController, true, null);
                                });
                    }
                });

            return;
        }

        private static UIKit.UIViewController GetVisibleViewController()
        {
            UIKit.UIViewController vc = null;

            var window = UIKit.UIApplication.SharedApplication.KeyWindow;
            vc = window.RootViewController;
            while (vc.PresentedViewController != null)
            {
                vc = vc.PresentedViewController;
            }

            return vc;
        }
    }
}
