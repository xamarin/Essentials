using System;
using System.Linq;
using System.Threading.Tasks;
using Android.Content;
using Android.Content.PM;

namespace Xamarin.Essentials.Launcher
{
    public static partial class Launcher
    {
        static Task<bool> PlatformCanOpenAsync(string uri)
        {
            var intent = new Intent(uri);
            var manager = Platform.AppContext.PackageManager;
            var supportedResolvedInfos = manager.QueryIntentActivities(intent, PackageInfoFlags.MatchDefaultOnly);
            return Task.FromResult(supportedResolvedInfos.Any());
        }

        static Task<bool> PlatformCanOpenAsync(Uri uri)
        {
            var intent = new Intent(uri.ToString());
            var manager = Platform.AppContext.PackageManager;
            var supportedResolvedInfos = manager.QueryIntentActivities(intent, PackageInfoFlags.MatchDefaultOnly);
            return Task.FromResult(supportedResolvedInfos.Any());
        }

        static Task PlatformOpenAsync(string uri)
        {
            var intent = new Intent(uri);
            Platform.AppContext.StartActivity(intent);
            return Task.CompletedTask;
        }

        static Task PlatformOpenAsync(Uri uri)
        {
            var intent = new Intent(uri.ToString());
            Platform.AppContext.StartActivity(intent);
            return Task.CompletedTask;
        }
    }
}
