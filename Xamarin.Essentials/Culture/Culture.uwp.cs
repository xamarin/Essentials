using System;
using System.Globalization;

namespace Xamarin.Essentials
{
    public static partial class Culture
    {
        static string PlatformInstalledUICulture =>
            CultureInfo.InstalledUICulture.Name;

        static CultureInfo PlatformGetCurrentUICulture(Func<string, CultureInfo> mappingOverride) =>
            CultureInfo.CurrentUICulture;

        static void PlatformSetCurrentUICulture(CultureInfo cultureInfo)
        {
            CultureInfo.CurrentCulture = cultureInfo;
            CultureInfo.CurrentUICulture = cultureInfo;
            Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = cultureInfo.Name;
            Windows.ApplicationModel.Resources.Core.ResourceContext.GetForCurrentView().Reset();
            Windows.ApplicationModel.Resources.Core.ResourceContext.GetForViewIndependentUse().Reset();
        }
    }
}
