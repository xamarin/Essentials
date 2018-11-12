using System.Collections.Generic;
using Xamarin.Essentials;

namespace Samples.ViewModel
{
    public class DeviceInfoViewModel : BaseViewModel
    {
        ScreenMetrics screenMetrics;
        List<StorageInfo> storageInfo;

        public string Model => DeviceInfo.Model;

        public string Manufacturer => DeviceInfo.Manufacturer;

        public string Name => DeviceInfo.Name;

        public string VersionString => DeviceInfo.VersionString;

        public string Version => DeviceInfo.Version.ToString();

        public string Platform => DeviceInfo.Platform;

        public string Idiom => DeviceInfo.Idiom;

        public List<StorageInfo> StorageInformation
        {
            get => storageInfo;
            set => SetProperty(ref storageInfo, value);
        }

        public DeviceType DeviceType => DeviceInfo.DeviceType;

        public ScreenMetrics ScreenMetrics
        {
            get => screenMetrics;
            set => SetProperty(ref screenMetrics, value);
        }

        public async override void OnAppearing()
        {
            base.OnAppearing();

            DeviceDisplay.ScreenMetricsChanged += OnScreenMetricsChanged;
            ScreenMetrics = DeviceDisplay.ScreenMetrics;
            StorageInformation = await DeviceInfo.GetStorageInformationAsync();
            OnPropertyChanged(nameof(StorageInformation));
        }

        public override void OnDisappearing()
        {
            DeviceDisplay.ScreenMetricsChanged -= OnScreenMetricsChanged;

            base.OnDisappearing();
        }

        void OnScreenMetricsChanged(object sender, ScreenMetricsChangedEventArgs e)
        {
            ScreenMetrics = e.Metrics;
        }
    }
}
