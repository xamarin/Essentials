using System;
using System.Threading;
using System.Windows.Threading;

namespace Xamarin.Essentials
{
    public static partial class MainThread
    {
        // find a safe way to do this for both WPF and WinForms
        static void PlatformBeginInvokeOnMainThread(Action action) => Dispatcher.CurrentDispatcher.BeginInvoke(action);

        static bool PlatformIsMainThread => Dispatcher.FromThread(Thread.CurrentThread) != null;
    }
}
