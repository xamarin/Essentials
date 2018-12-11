using Xamarin.Essentials;

namespace Samples.ViewModel
{
    public class ConnectivityViewModel : BaseViewModel
    {
        public string NetworkAccess =>
            Connectivity.NetworkAccess.ToString();
            
        public string SignalStrength => Connectivity.WifiSignalStrength.ToString();

        public string Profiles =>
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
