using System.Linq;
using System.Threading.Tasks;
using Android.Content;
using Android.Content.PM;
using AndroidUri = Android.Net.Uri;
using Uri = System.Uri;

namespace Xamarin.Essentials
{
    public static partial class Launcher
    {
        static Task<bool> PlatformCanOpenAsync(Uri uri)
        {
            var intent = CreateIntent(uri.AbsoluteUri);
            var manager = Platform.AppContext.PackageManager;
            var supportedResolvedInfos = manager.QueryIntentActivities(intent, PackageInfoFlags.MatchDefaultOnly);
            return Task.FromResult(supportedResolvedInfos.Any());
        }

        static Task PlatformOpenAsync(Uri uri)
        {
            var intent = CreateIntent(uri.AbsoluteUri);
            Platform.AppContext.StartActivity(intent);
            return Task.CompletedTask;
        }

        static Intent CreateIntent(string uri)
            => new Intent(Intent.ActionView, AndroidUri.Parse(uri));
    }
}
