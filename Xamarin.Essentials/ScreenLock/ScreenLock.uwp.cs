using Windows.System.Display;

namespace Xamarin.Essentials
{
    public static partial class ScreenLock
    {
        private static readonly object locker = new object();
        private static DisplayRequest displayRequest;

        public static bool IsActive
        {
            get
            {
                lock (locker)
                {
                    return displayRequest != null;
                }
            }
        }

        public static void RequestActive()
        {
            lock (locker)
            {
                if (displayRequest == null)
                {
                    displayRequest = new DisplayRequest();
                    displayRequest.RequestActive();
                }
            }
        }

        public static void RequestRelease()
        {
            lock (locker)
            {
                if (displayRequest != null)
                {
                    displayRequest.RequestRelease();
                    displayRequest = null;
                }
            }
        }
    }
}
