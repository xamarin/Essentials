using System.Collections.ObjectModel;
using Caboodle.Samples.Model;
using Caboodle.Samples.View;

namespace Caboodle.Samples.ViewModel
{
    public class HomeViewModel : BaseViewModel
    {
        public HomeViewModel()
        {
            Items = new ObservableCollection<SampleItem>
            {
                new SampleItem("Battery", typeof(BatteryPage), "Easily detect battery level, source, and state."),
                new SampleItem("Clipboard", typeof(ClipboardPage), "Set and get text from the clipboard."),
                new SampleItem("Connectivity", typeof(ConnectivityPage), "Check connectivity state and detect changes."),
                new SampleItem("Device Info", typeof(DeviceInfoPage), "Find out about the device with ease."),
                new SampleItem("File System", typeof(FileSystemPage), "Easily save files to app data."),
                new SampleItem("Geocoding", typeof(GeocodingPage), "Easily geocode and reverse geocode."),
                new SampleItem("Preferences", typeof(PreferencesPage), "Quickly and easily add persistent preferences.")
            };
        }

        public ObservableCollection<SampleItem> Items { get; }
    }
}
