using System;
using System.Threading.Tasks;
using Windows.System;
using WinLauncher = Windows.System.Launcher;

namespace Xamarin.Essentials
{
    public static partial class Launcher
    {
        public static Task<bool> PlatformCanOpenAsync(string uri)
        {
            return PlatformCanOpenAsync(new Uri(uri));
        }

        public static async Task<bool> PlatformCanOpenAsync(Uri uri)
        {
            var supported = await WinLauncher.QueryUriSupportAsync(uri, LaunchQuerySupportType.Uri);
            return supported == LaunchQuerySupportStatus.Available;
        }

        public static Task PlatformOpenAsync(string uri)
        {
            return PlatformOpenAsync(new Uri(uri));
        }

        public static async Task PlatformOpenAsync(Uri uri)
        {
            await WinLauncher.LaunchUriAsync(uri);
        }
    }
}
