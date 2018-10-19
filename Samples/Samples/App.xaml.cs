using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Distribute;
using Samples.View;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Device = Xamarin.Forms.Device;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace Samples
{
    public partial class App : Application
    {
        const string appCenterAndroid = "AC_ANDROID";
        const string appCenteriOS = "AC_IOS";
        const string appCenterUWP = "AC_UWP";

        public App()
        {
            InitializeComponent();

            VersionTracking.Track();

            MainPage = new NavigationPage(new HomePage());
        }

        protected override void OnStart()
        {
            if ((Device.RuntimePlatform == Device.Android && appCenterAndroid != "AC_ANDROID") ||
               (Device.RuntimePlatform == Device.iOS && appCenteriOS != "AC_IOS") ||
               (Device.RuntimePlatform == Device.UWP && appCenterUWP != "AC_UWP"))
            {
                AppCenter.Start(
                $"ios={appCenteriOS};" +
                $"android={appCenterAndroid};" +
                $"uwp={appCenterUWP}",
                typeof(Analytics),
                typeof(Crashes),
                typeof(Distribute));
            }

            // set UWP Map Key
            Geocoding.MapKey = "RJHqIE53Onrqons5CNOx~FrDr3XhjDTyEXEjng-CRoA~Aj69MhNManYUKxo6QcwZ0wmXBtyva0zwuHB04rFYAPf7qqGJ5cHb03RCDw1jIW8l";
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
