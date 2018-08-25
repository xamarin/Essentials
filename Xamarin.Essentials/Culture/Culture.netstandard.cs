using System;
using System.Globalization;

namespace Xamarin.Essentials
{
    public static partial class Culture
    {
        internal static string PlatformInstalledUICulture =>
            throw new NotImplementedInReferenceAssemblyException();

        internal static CultureInfo PlatformGetCurrentUICulture() =>
            throw new NotImplementedInReferenceAssemblyException();

        internal static CultureInfo PlatformGetCurrentUICulture(Func<string, CultureInfo> mappingOverride) =>
            throw new NotImplementedInReferenceAssemblyException();

        internal static void PlatformSetCurrentUICulture(CultureInfo cultureInfo) =>
            throw new NotImplementedInReferenceAssemblyException();
    }
}
