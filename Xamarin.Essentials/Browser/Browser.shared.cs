using System;
using System.Threading.Tasks;

namespace Xamarin.Essentials
{
    public static partial class Browser
    {
        public static Task OpenAsync(string uri) =>
            OpenAsync(uri, BrowserLaunchType.SystemPreferred);

        public static Task OpenAsync(string uri, BrowserLaunchType launchType)
        {
            if (string.IsNullOrWhiteSpace(uri))
            {
                throw new ArgumentNullException(nameof(uri), $"Uri cannot be empty.");
            }

            return OpenAsync(new Uri(uri), launchType);
        }

        public static Task OpenAsync(Uri uri) =>
          OpenAsync(uri, BrowserLaunchType.SystemPreferred);

        public static Task OpenAsync(Uri uri, BrowserLaunchType launchType)
        {
#if NETSTANDARD1_0
            return PlatformOpenAsync(uri, launchType);
#else
            var idn = new System.Globalization.IdnMapping();
            return PlatformOpenAsync(new Uri(uri.Scheme + "://" + idn.GetAscii(uri.DnsSafeHost) + uri.PathAndQuery), launchType);
#endif
        }
    }

    public enum BrowserLaunchType
    {
        External,
        SystemPreferred
    }
}
