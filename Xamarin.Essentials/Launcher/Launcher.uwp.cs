using System;
using System.Threading.Tasks;
using Windows.System;
using WinLauncher = Windows.System.Launcher;

namespace Xamarin.Essentials
{
    public static partial class Launcher
    {
        public static async Task<bool> PlatformCanOpenAsync(string uri)
        {
            var supported = await WinLauncher.QueryUriSupportAsync(new Uri(uri), LaunchQuerySupportType.Uri | LaunchQuerySupportType.UriForResults);
            return supported == LaunchQuerySupportStatus.Available;
        }

        public static async Task<bool> PlatformCanOpenAsync(Uri uri)
        {
            var supported = await WinLauncher.QueryUriSupportAsync(uri, LaunchQuerySupportType.Uri | LaunchQuerySupportType.UriForResults);
            return supported == LaunchQuerySupportStatus.Available;
        }

        public static async Task PlatformOpenAsync(string uri)
        {
            await WinLauncher.LaunchUriAsync(new Uri(uri));
        }

        public static async Task PlatformOpenAsync(Uri uri)
        {
            await WinLauncher.LaunchUriAsync(uri);
        }
    }
}
