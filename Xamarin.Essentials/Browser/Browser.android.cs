using System;
using System.Threading.Tasks;

using Android.Content;
using Android.Support.CustomTabs;

using AndroidUri = Android.Net.Uri;

namespace Xamarin.Essentials
{
    public static partial class Browser
    {
        static Task<bool> PlatformOpenAsync(Uri uri, BrowserLaunchOptions options)
        {
            var nativeUri = AndroidUri.Parse(uri.AbsoluteUri);

            switch (options.LaunchMode)
            {
                case BrowserLaunchMode.SystemPreferred:
                    var tabsBuilder = new CustomTabsIntent.Builder();
                    tabsBuilder.SetShowTitle(true);
                    if (options.PreferredTitleColor.HasValue)
                        tabsBuilder.SetToolbarColor((int)options.PreferredTitleColor.Value.ToInt());
                    if (options.PrefferedControlColor.HasValue)
                        tabsBuilder.SetSecondaryToolbarColor((int)options.PrefferedControlColor.Value.ToInt());
                    if (options.TitleMode != BrowserTitleMode.Default)
                        tabsBuilder.SetShowTitle(options.TitleMode == BrowserTitleMode.Show);
                    var tabsIntent = tabsBuilder.Build();
                    tabsIntent.Intent.SetFlags(ActivityFlags.ClearTop);
                    tabsIntent.Intent.SetFlags(ActivityFlags.NewTask);
                    tabsIntent.LaunchUrl(Platform.AppContext, nativeUri);
                    break;
                case BrowserLaunchMode.External:
                    var intent = new Intent(Intent.ActionView, nativeUri);
                    intent.SetFlags(ActivityFlags.ClearTop);
                    intent.SetFlags(ActivityFlags.NewTask);

                    if (!Platform.IsIntentSupported(intent))
                        throw new FeatureNotSupportedException();

                    Platform.AppContext.StartActivity(intent);
                    break;
            }

            return Task.FromResult(true);
        }
    }
}
