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
            var intent = CreateIntent(uri.ToString());
            var manager = Platform.AppContext.PackageManager;
            var supportedResolvedInfos = manager.QueryIntentActivities(intent, PackageInfoFlags.MatchDefaultOnly);
            return Task.FromResult(supportedResolvedInfos.Any());
        }

        static Task PlatformOpenAsync(Uri uri)
        {
            var intent = CreateIntent(uri.ToString());
            Platform.AppContext.StartActivity(intent);
            return Task.CompletedTask;
        }

        static Intent CreateIntent(string uri)
        {
            var androidUri = AndroidUri.Parse(uri);
            var intent = new Intent(Intent.ActionView, androidUri);
            return intent;
        }
    }
}
