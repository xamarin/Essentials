using System;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;

namespace Xamarin.Essentials
{
    public static partial class Platform
    {
        static bool PlatformIsMainThread =>
            CoreApplication.MainView.CoreWindow.Dispatcher.HasThreadAccess;

        static void PlatformBeginInvokeOnMainThread(Action action)
        {
            var dispatcher = CoreApplication.MainView.CoreWindow.Dispatcher;

            if (!dispatcher.HasThreadAccess)
                dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => action()).WatchForError();
            else
                action();
        }
    }
}
