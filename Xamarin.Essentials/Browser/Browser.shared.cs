using System;
using System.Threading.Tasks;

namespace Xamarin.Essentials
{
    public static partial class Browser
    {
        public static Task OpenAsync(string uri, BrowserLaunchOptions options = default) =>
            OpenAsync(uri, BrowserLaunchMode.SystemPreferred, options);

        public static Task OpenAsync(string uri, BrowserLaunchMode launchMode, BrowserLaunchOptions options = default)
        {
            if (string.IsNullOrWhiteSpace(uri))
            {
                throw new ArgumentNullException(nameof(uri), $"Uri cannot be empty.");
            }

            return OpenAsync(new Uri(uri), launchMode);
        }

        public static Task OpenAsync(Uri uri, BrowserLaunchOptions options = default) =>
          OpenAsync(uri, BrowserLaunchMode.SystemPreferred, options);

        public static Task<bool> OpenAsync(Uri uri, BrowserLaunchMode launchMode, BrowserLaunchOptions options = default) =>
            PlatformOpenAsync(EscapeUri(uri), launchMode, options);

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

    public readonly partial struct BrowserLaunchOptions : IEquatable<BrowserLaunchOptions>
    {
        public BrowserLaunchOptions(EssentialsColor? prefferedControlColor, EssentialsColor? preferredBackgroundColor, EssentialsColor? preferredTitleColor, bool? shouldShowTitle)
        {
            PrefferedControlColor = prefferedControlColor;
            PreferredBackgroundColor = preferredBackgroundColor;
            PreferredTitleColor = preferredTitleColor;
            ShouldShowTitle = shouldShowTitle;
        }

        public EssentialsColor? PreferredTitleColor { get; }

        public EssentialsColor? PreferredBackgroundColor { get; }

        public EssentialsColor? PrefferedControlColor { get; }

        public bool? ShouldShowTitle { get; }

        public static bool operator ==(BrowserLaunchOptions lhs, BrowserLaunchOptions rhs) => lhs.Equals(rhs);

        public static bool operator !=(BrowserLaunchOptions lhs, BrowserLaunchOptions rhs) => lhs.Equals(rhs);

        public override bool Equals(object other) => other is BrowserLaunchOptions options && Equals(options);

        public bool Equals(BrowserLaunchOptions other) => PreferredTitleColor.Equals(other.PreferredTitleColor)
                                                          && PreferredBackgroundColor.Equals(other.PreferredBackgroundColor)
                                                          && PrefferedControlColor.Equals(other.PrefferedControlColor)
                                                          && ShouldShowTitle.Equals(other.ShouldShowTitle);

        public override int GetHashCode() => (PreferredBackgroundColor, PreferredBackgroundColor, PrefferedControlColor, ShouldShowTitle).GetHashCode();
    }

    public readonly partial struct EssentialsColor : IEquatable<EssentialsColor>
    {
        public byte R { get; }

        public byte G { get; }

        public byte B { get; }

        public byte A { get; }

        public static bool operator ==(EssentialsColor lhs, EssentialsColor rhs) => lhs.Equals(rhs);

        public static bool operator !=(EssentialsColor lhs, EssentialsColor rhs) => lhs.Equals(rhs);

        public override bool Equals(object other) => other is EssentialsColor color && Equals(color);

        public bool Equals(EssentialsColor other) =>
            A.Equals(other.A) && R.Equals(other.R) && G.Equals(other.G) && B.Equals(other.B);

        public override int GetHashCode() => (A, R, G, B).GetHashCode();
    }

    public enum BrowserLaunchMode
    {
        SystemPreferred = 0,
        External = 1,
    }
}
