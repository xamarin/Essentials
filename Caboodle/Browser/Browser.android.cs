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

            await Browser.OpenAsync(new System.Uri(uri), BrowserLaunchingType.UriLauncher);

            return;
        }

        public static async Task OpenAsync(string uri, BrowserLaunchingType launch_type)
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
            await OpenAsync(uri, BrowserLaunchingType.UriLauncher);

            return;
        }

        public static async Task OpenAsync(System.Uri uri, BrowserLaunchingType launch_type)
        {
            await Task.Run(() =>
            {
                if (uri == null)
                {
                    throw new ArgumentNullException($"Uri cannot be null.");
                }

                var uri_native = global::Android.Net.Uri.Parse(uri.OriginalString);

                if (launch_type == BrowserLaunchingType.UriLauncher)
                {
                    var intent = new Android.Content.Intent(Android.Content.Intent.ActionView, uri_native);
                    intent.SetFlags(Android.Content.ActivityFlags.ClearTop);
                    intent.SetFlags(Android.Content.ActivityFlags.NewTask);
                    Platform.CurrentContext.StartActivity(intent);
                }
                else if (launch_type == BrowserLaunchingType.SystemBrowser)
                {
                    /*
                     motz's code UI customization
                     var toolbarColor = options?.ChromeToolbarColor;
                     if (toolbarColor != null)
                         tabsBuilder.SetToolbarColor(toolbarColor.ToNativeColor());
                    */

                    var tabsBuilder = new Android.Support.CustomTabs.CustomTabsIntent.Builder();
                    var intent = tabsBuilder.Build();
                    tabsBuilder.SetShowTitle(true);
                    intent.LaunchUrl(Platform.CurrentContext, uri_native);

                    // chrome custom tabs will fall back to normal launching of browser if they don't exist
                }

                return;
            });

            return;
        }
    }
}
