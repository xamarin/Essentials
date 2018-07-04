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
        public static Task<bool> PlatformCanOpenAsync(string uri)
        {
            var intent = CreateIntent(uri);
            var manager = Platform.AppContext.PackageManager;
            var supportedResolvedInfos = manager.QueryIntentActivities(intent, PackageInfoFlags.MatchDefaultOnly);
            return Task.FromResult(supportedResolvedInfos.Any());
        }

        public static Task<bool> PlatformCanOpenAsync(Uri uri)
        {
            return PlatformCanOpenAsync(uri.ToString());
        }

        public static Task PlatformOpenAsync(string uri)
        {
            var intent = CreateIntent(uri);
            Platform.AppContext.StartActivity(intent);
            return Task.CompletedTask;
        }

        public static Task PlatformOpenAsync(Uri uri)
        {
            return PlatformOpenAsync(uri.ToString());
        }

        static Intent CreateIntent(string uri)
        {
            var androidUri = AndroidUri.Parse(uri);
            var intent = new Intent(Intent.ActionView, androidUri);
            return intent;
        }
    }
}
