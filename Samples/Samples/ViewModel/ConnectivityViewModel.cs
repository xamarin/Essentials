using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Samples.ViewModel
{
    public class ConnectivityViewModel : BaseViewModel
    {
        public string NetworkAccess =>
            Connectivity.NetworkAccess.ToString();

        ICommand refreshCommand;

        public ICommand RefreshCommand => refreshCommand ?? (refreshCommand = new Command(() => OnConnectivityChanged(null, null)));

        public string SignalStrength => Connectivity.WiFi.SignalStrength.ToString();

        public string ConnectionProfiles =>
            string.Join("\n", Connectivity.ConnectionProfiles);

        public override void OnAppearing()
        {
            base.OnAppearing();

            Connectivity.ConnectivityChanged += OnConnectivityChanged;
        }

        public override void OnDisappearing()
        {
            Connectivity.ConnectivityChanged -= OnConnectivityChanged;

            base.OnDisappearing();
        }

        void OnConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            OnPropertyChanged(nameof(ConnectionProfiles));
            OnPropertyChanged(nameof(NetworkAccess));
            OnPropertyChanged(nameof(SignalStrength));
        }
    }
}
