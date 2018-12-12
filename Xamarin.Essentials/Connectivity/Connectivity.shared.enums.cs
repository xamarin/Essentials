namespace Xamarin.Essentials
{
    public enum ConnectionProfile
    {
        Unknown = 0,
        Bluetooth = 1,
        Cellular = 2,
        Ethernet = 3,
        WiFi = 4
    }

    public enum NetworkAccess
    {
        Unknown = 0,
        None = 1,
        Local = 2,
        ConstrainedInternet = 3,
        Internet = 4
    }

    public enum SignalStrength
    {
        Unknown = 0,
        None = 1,
        Poor = 2,
        Moderate = 3,
        Good = 4,
        Great = 5
    }
}
