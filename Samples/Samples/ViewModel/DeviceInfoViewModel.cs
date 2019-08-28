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

        public string Architectures
        {
            get
            {
                var info = DeviceInfo.Architectures;
                if (info.Length <= 1)
                    return info[0];

                var architectures = string.Empty;
                foreach (var architecture in info)
                {
                    architectures += architecture + ", ";
                }
                return architectures.Remove(architectures.Length - 2);
            }
        }

        public DevicePlatform Platform => DeviceInfo.Platform;

        public DeviceIdiom Idiom => DeviceInfo.Idiom;

        public DeviceType DeviceType => DeviceInfo.DeviceType;

        public DisplayInfo ScreenMetrics
        {
            get => screenMetrics;
            set => SetProperty(ref screenMetrics, value);
        }

        public override void OnAppearing()
        {
            base.OnAppearing();
            DeviceDisplay.MainDisplayInfoChanged += OnScreenMetricsChanged;
            ScreenMetrics = DeviceDisplay.MainDisplayInfo;
        }

        public override void OnDisappearing()
        {
            DeviceDisplay.MainDisplayInfoChanged -= OnScreenMetricsChanged;

            base.OnDisappearing();
        }

        void OnScreenMetricsChanged(object sender, DisplayInfoChangedEventArgs e)
        {
            ScreenMetrics = e.DisplayInfo;
        }
    }
}
