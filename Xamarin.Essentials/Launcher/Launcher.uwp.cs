using System;
using System.Threading.Tasks;
using Windows.System;
using WinLauncher = Windows.System.Launcher;

namespace Xamarin.Essentials
{
    public static partial class Launcher
    {
        static async Task<bool> PlatformCanOpenAsync(Uri uri)
        {
            var supported = await WinLauncher.QueryUriSupportAsync(uri, LaunchQuerySupportType.Uri);
            return supported == LaunchQuerySupportStatus.Available;
        }

        static async Task PlatformOpenAsync(Uri uri)
        {
            await WinLauncher.LaunchUriAsync(uri);
        }
    }
}
