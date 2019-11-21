using System.Collections.Generic;
using Xamarin.Essentials;

namespace Samples.ViewModel
{
    public class DeviceInfoViewModel : BaseViewModel
    {
        DisplayInfo screenMetrics;
        List<StorageInfo> storageInfo;

        public string Model => DeviceInfo.Model;

        public string Manufacturer => DeviceInfo.Manufacturer;

        public string Name => DeviceInfo.Name;

        public string VersionString => DeviceInfo.VersionString;

        public string Version => DeviceInfo.Version.ToString();

        public DevicePlatform Platform => DeviceInfo.Platform;

        public DeviceIdiom Idiom => DeviceInfo.Idiom;

        public List<StorageInfo> StorageInformation
        {
            get => storageInfo;
            set => SetProperty(ref storageInfo, value);
        }

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
            StorageInformation = await DeviceInfo.GetStorageInformationAsync();
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
