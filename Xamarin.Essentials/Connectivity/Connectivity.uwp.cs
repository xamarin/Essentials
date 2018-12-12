using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Windows.Devices.WiFi;
using Windows.Networking.Connectivity;

namespace Xamarin.Essentials
{
    public static partial class Connectivity
    {
        static void StartListeners() =>
             NetworkInformation.NetworkStatusChanged += NetworkStatusChanged;

        static void NetworkStatusChanged(object sender) =>
            OnConnectivityChanged();

        static void StopListeners() =>
             NetworkInformation.NetworkStatusChanged -= NetworkStatusChanged;

        static SignalStrength PlatformWiFiSignalStrength()
        {
            var signalStrength = NetworkInformation.GetConnectionProfiles()
                .Where(profile => profile?.IsWlanConnectionProfile ?? false)
                .Select(profile => profile.GetSignalBars())
                .Where(signalBar => signalBar.HasValue)
                .OrderBy(strength => strength)
                .FirstOrDefault();

            if (!signalStrength.HasValue)
                return SignalStrength.Unknown;

            // An integer value within a range of 0-5 that corresponds to the number of signal bars displayed by the UI.
            // https://docs.microsoft.com/en-us/uwp/api/windows.networking.connectivity.connectionprofile.getsignalbars#Windows_Networking_Connectivity_ConnectionProfile_GetSignalBars

            switch (signalStrength.Value)
            {
                case 0:
                    return SignalStrength.None;
                case 1:
                    return SignalStrength.Poor;
                case 2:
                    return SignalStrength.Moderate;
                case 3:
                    return SignalStrength.Good;
                case 4:
                case 5:
                    return SignalStrength.Great;
                default:
                    Debug.WriteLine($"Invalid signal strength encountered: {signalStrength.Value}");
                    return SignalStrength.Unknown;
            }
        }

        static NetworkAccess PlatformNetworkAccess
        {
            get
            {
                var profile = NetworkInformation.GetInternetConnectionProfile();
                if (profile == null)
                    return NetworkAccess.Unknown;

                var level = profile.GetNetworkConnectivityLevel();
                switch (level)
                {
                    case NetworkConnectivityLevel.LocalAccess:
                        return NetworkAccess.Local;
                    case NetworkConnectivityLevel.InternetAccess:
                        return NetworkAccess.Internet;
                    case NetworkConnectivityLevel.ConstrainedInternetAccess:
                        return NetworkAccess.ConstrainedInternet;
                    default:
                        return NetworkAccess.None;
                }
            }
        }

        static IEnumerable<ConnectionProfile> PlatformConnectionProfiles
        {
            get
            {
                var networkInterfaceList = NetworkInformation.GetConnectionProfiles();
                foreach (var interfaceInfo in networkInterfaceList.Where(nii => nii.GetNetworkConnectivityLevel() != NetworkConnectivityLevel.None))
                {
                    var type = ConnectionProfile.Unknown;

                    if (interfaceInfo.NetworkAdapter != null)
                    {
                        // http://www.iana.org/assignments/ianaiftype-mib/ianaiftype-mib
                        switch (interfaceInfo.NetworkAdapter.IanaInterfaceType)
                        {
                            case 6:
                                type = ConnectionProfile.Ethernet;
                                break;
                            case 71:
                                type = ConnectionProfile.WiFi;
                                break;
                            case 243:
                            case 244:
                                type = ConnectionProfile.Cellular;
                                break;

                            // xbox wireless, can skip
                            case 281:
                                continue;
                        }
                    }

                    yield return type;
                }
            }
        }
    }
}
