using System;

namespace Xamarin.Essentials
{
    public static partial class MainThread
    {
        static void PlatformBeginInvokeOnMainThread(Action action) =>
            ThrowHelper.ThrowNotImplementedException();

        static bool PlatformIsMainThread =>
            ThrowHelper.ThrowNotImplementedException<bool>();
    }
}
