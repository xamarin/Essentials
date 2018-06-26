using System.Globalization;

namespace Xamarin.Essentials
{
    public static partial class Culture
    {
        public static CultureInfo PlatformCurrent =>
            CultureInfo.CurrentUICulture;

        public static void PlatformSetLocale(CultureInfo cultureInfo)
        {
            CultureInfo.CurrentCulture = cultureInfo;
            CultureInfo.CurrentUICulture = cultureInfo;
            Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = cultureInfo.Name;
            Windows.ApplicationModel.Resources.Core.ResourceContext.GetForCurrentView().Reset();
            Windows.ApplicationModel.Resources.Core.ResourceContext.GetForViewIndependentUse().Reset();
        }
    }
}
