using System;
using System.Threading.Tasks;
using Windows.System;
using WinLauncher = Windows.System.Launcher;

namespace Xamarin.Essentials.Launcher
{
    public static partial class Launcher
    {
        static async Task<bool> PlatformCanOpenAsync(string uri)
        {
            var supported = await WinLauncher.QueryUriSupportAsync(new Uri(uri), LaunchQuerySupportType.Uri | LaunchQuerySupportType.UriForResults);
            return supported == LaunchQuerySupportStatus.Available;
        }

        static async Task<bool> PlatformCanOpenAsync(Uri uri)
        {
            var supported = await WinLauncher.QueryUriSupportAsync(uri, LaunchQuerySupportType.Uri | LaunchQuerySupportType.UriForResults);
            return supported == LaunchQuerySupportStatus.Available;
        }

        static async Task PlatformOpenAsync(string uri)
        {
            await WinLauncher.LaunchUriAsync(new Uri(uri));
        }

        static async Task PlatformOpenAsync(Uri uri)
        {
            await WinLauncher.LaunchUriAsync(uri);
        }
    }
}
