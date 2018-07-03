using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.Essentials
{
    public static partial class Launcher
    {
        static Task<bool> CanOpenAsync(string uri) => PlatformCanOpenAsync(uri);

        static Task<bool> CanOpenAsync(Uri uri) => PlatformCanOpenAsync(uri);

        static Task OpenAsync(string uri) => PlatformOpenAsync(uri);

        static Task OpenAsync(Uri uri) => PlatformOpenAsync(uri);
    }
}
