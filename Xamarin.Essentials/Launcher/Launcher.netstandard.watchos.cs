using System;
using System.Threading.Tasks;

namespace Xamarin.Essentials
{
    public static partial class Launcher
    {
        static Task<bool> PlatformCanOpenAsync(Uri uri) =>
            ThrowHelper.ThrowNotImplementedException<Task<bool>>();

        static Task PlatformOpenAsync(Uri uri) =>
            ThrowHelper.ThrowNotImplementedException<Task>();

        static Task PlatformOpenAsync(OpenFileRequest request) =>
            ThrowHelper.ThrowNotImplementedException<Task>();

        static Task<bool> PlatformTryOpenAsync(Uri uri) =>
            ThrowHelper.ThrowNotImplementedException<Task<bool>>();
    }
}
