using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

#if NETSTANDARD1_0
public struct Color
{
    public int Value { get; set; }

    public byte A => (byte)(Value >> 24);

    public byte R => (byte)(Value >> 16);

    public byte G => (byte)(Value >> 8);

    public byte B => (byte)Value;
}
#else
using System.Drawing;
[assembly: TypeForwardedTo(typeof(Color))]
#endif

namespace Xamarin.Essentials
{
    public static partial class Browser
    {
        public static Task OpenAsync(string uri) =>
            OpenAsync(uri, new BrowserLaunchOptions());

        public static Task OpenAsync(string uri, BrowserLaunchMode launchMode) =>
            OpenAsync(uri, new BrowserLaunchOptions());

        public static Task OpenAsync(string uri, BrowserLaunchOptions options)
        {
            if (string.IsNullOrWhiteSpace(uri))
            {
                throw new ArgumentNullException(nameof(uri), $"Uri cannot be empty.");
            }

            return OpenAsync(new Uri(uri), options);
        }

        public static Task OpenAsync(Uri uri) =>
            OpenAsync(uri, new BrowserLaunchOptions());

        public static Task OpenAsync(Uri uri, BrowserLaunchMode launchMode) =>
            OpenAsync(uri, new BrowserLaunchOptions(launchMode));

        public static Task<bool> OpenAsync(Uri uri, BrowserLaunchOptions options) =>
            PlatformOpenAsync(EscapeUri(uri), options);

        internal static Uri EscapeUri(Uri uri)
        {
#if NETSTANDARD1_0
            return uri;
#else
            var idn = new System.Globalization.IdnMapping();
            return new Uri(uri.Scheme + "://" + idn.GetAscii(uri.Authority) + uri.PathAndQuery);
#endif
        }
    }

    public class BrowserLaunchOptions : IEquatable<BrowserLaunchOptions>
    {
        public BrowserLaunchOptions()
        {
            LaunchMode = BrowserLaunchMode.Default;
        }

        public BrowserLaunchOptions(BrowserLaunchMode mode)
        {
            LaunchMode = mode;
        }

        public Color? PreferredTitleColor { get; set; }

        public Color? PreferredBackgroundColor { get; set; }

        public Color? PrefferedControlColor { get; set; }

        public BrowserLaunchMode LaunchMode { get; set; }

        public BrowserTitleMode TitleMode { get; set; }

        public static bool operator ==(BrowserLaunchOptions lhs, BrowserLaunchOptions rhs) => (lhs == null && rhs == null) || (lhs != null && lhs.Equals(rhs));

        public static bool operator !=(BrowserLaunchOptions lhs, BrowserLaunchOptions rhs) => !(lhs == rhs);

        public override bool Equals(object other) => other is BrowserLaunchOptions options && Equals(options);

        public bool Equals(BrowserLaunchOptions other) => other != null && PreferredTitleColor.Equals(other.PreferredTitleColor)
                                                          && PreferredBackgroundColor.Equals(
                                                              other.PreferredBackgroundColor)
                                                          && PrefferedControlColor.Equals(other.PrefferedControlColor)
                                                          && LaunchMode.Equals(other.LaunchMode)
                                                          && TitleMode.Equals(other.TitleMode);

        public override int GetHashCode() => (PreferredBackgroundColor, PreferredBackgroundColor, PrefferedControlColor,
            TitleMode, LaunchMode).GetHashCode();
    }

    public enum BrowserTitleMode
    {
        Default = 0,
        Show = 1,
        Hide = 2
    }

    public enum BrowserLaunchMode
    {
        Default = 0,
        SystemPreferred = 1,
        External = 2
    }
}
