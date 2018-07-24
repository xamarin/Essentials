using System;
using System.Threading.Tasks;

namespace Xamarin.Essentials
{
    public static partial class Browser
    {
        public static Task OpenAsync(string uri) =>
            OpenAsync(uri, BrowserLaunchMode.SystemPreferred);

        public static Task OpenAsync(string uri, BrowserLaunchMode launchType)
        {
            if (string.IsNullOrWhiteSpace(uri))
            {
                throw new ArgumentNullException(nameof(uri), $"Uri cannot be empty.");
            }

            return OpenAsync(new Uri(uri), launchType);
        }

        public static Task OpenAsync(Uri uri) =>
          OpenAsync(uri, BrowserLaunchMode.SystemPreferred);

        public static Task OpenAsync(Uri uri, BrowserLaunchMode launchType) =>
            PlatformOpenAsync(EscapeUri(uri), launchType);

        internal static Uri EscapeUri(Uri uri)
        {
#if NETSTANDARD1_0
            return uri;
#else
            var idn = new System.Globalization.IdnMapping();
            return new Uri(uri.Scheme + "://" + idn.GetAscii(uri.DnsSafeHost) + uri.PathAndQuery);
#endif
        }
    }

    public enum BrowserLaunchMode
    {
        External,
        SystemPreferred
    }
}
