using System.Globalization;

namespace Xamarin.Essentials
{
    public static partial class Culture
    {
        internal static CultureInfo PlatformCurrent =>
            throw new NotImplementedInReferenceAssemblyException();

        internal static CultureInfo PlatformSetLocale(CultureInfo cultureInfo) =>
            throw new NotImplementedInReferenceAssemblyException();
    }
}
