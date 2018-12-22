using Xamarin.Essentials;

namespace Samples.ViewModel
{
    public class DeviceInfoViewModel : BaseViewModel
    {
        DisplayInfo screenMetrics;

        public string Model => DeviceInfo.Model;

        public string Manufacturer => DeviceInfo.Manufacturer;

        public string Name => DeviceInfo.Name;

        public string VersionString => DeviceInfo.VersionString;

        public string Version => DeviceInfo.Version.ToString();

        public DevicePlatform Platform => DeviceInfo.Platform;

        public DeviceIdiom Idiom => DeviceInfo.Idiom;

        public DeviceType DeviceType => DeviceInfo.DeviceType;

        public DisplayInfo ScreenMetrics
        {
            get => screenMetrics;
            set => SetProperty(ref screenMetrics, value);
        }

        public async override void OnAppearing()
        {
            base.OnAppearing();

            DeviceDisplay.MainDisplayInfoChanged += OnScreenMetricsChanged;
            ScreenMetrics = DeviceDisplay.MainDisplayInfo;
            DeviceInfo.ShakenLister = true;
            DeviceInfo.Shaken += DeviceInfo_Shaken;
            await DisplayAlertAsync("This page detect when device shaked!");
        }

        async void DeviceInfo_Shaken(object sender, System.EventArgs e)
        {
            await DisplayAlertAsync("Shaked");
        }

        public override void OnDisappearing()
        {
            DeviceDisplay.MainDisplayInfoChanged -= OnScreenMetricsChanged;
            DeviceInfo.ShakenLister = true;
            DeviceInfo.Shaken -= DeviceInfo_Shaken;

            base.OnDisappearing();
        }

        void OnScreenMetricsChanged(object sender, DisplayInfoChangedEventArgs e)
        {
            ScreenMetrics = e.DisplayInfo;
        }
    }
}
