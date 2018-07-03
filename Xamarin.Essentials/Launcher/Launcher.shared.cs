using System;
using System.Threading.Tasks;

namespace Xamarin.Essentials
{
    public static partial class Launcher
    {
        public static Task<bool> CanOpenAsync(string uri) => PlatformCanOpenAsync(uri);

        public static Task<bool> CanOpenAsync(Uri uri) => PlatformCanOpenAsync(uri);

        public static Task OpenAsync(string uri) => PlatformOpenAsync(uri);

        public static Task OpenAsync(Uri uri) => PlatformOpenAsync(uri);
    }
}
