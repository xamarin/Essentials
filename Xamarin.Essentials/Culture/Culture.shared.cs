using System;
using System.Globalization;

namespace Xamarin.Essentials
{
    public static partial class Culture
    {
        public static CultureInfo Current =>
            PlatformCurrent;

        public static void SetLocale(CultureInfo cultureInfo) =>
            PlatformSetLocale(cultureInfo);

        internal class PlatformCulture
        {
            internal PlatformCulture(string platformCultureString)
            {
                if (string.IsNullOrEmpty(platformCultureString))
                {
                    throw new ArgumentException("Expected culture identifier", "platformCultureString"); // in C# 6 use nameof(platformCultureString)
                }
                PlatformString = platformCultureString.Replace("_", "-"); // .NET expects dash, not underscore
                var dashIndex = PlatformString.IndexOf("-", StringComparison.Ordinal);
                if (dashIndex > 0)
                {
                    var parts = PlatformString.Split('-');
                    LanguageCode = parts[0];
                    LocaleCode = parts[1];
                }
                else
                {
                    LanguageCode = PlatformString;
                    LocaleCode = string.Empty;
                }
            }

            public string PlatformString { get; private set; }

            public string LanguageCode { get; private set; }

            public string LocaleCode { get; private set; }

            public override string ToString() => PlatformString;
        }
    }
}
