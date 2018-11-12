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

        static SignalStrength PlatformSignalStrength()
        {
            var signalStrength = NetworkInformation.GetConnectionProfiles()
                .Where(profile => profile != null && profile.IsWlanConnectionProfile)
                .Select(profile => profile.GetSignalBars())
                .Where(signalBar => signalBar.HasValue)
                .OrderBy(strength => strength)
                .FirstOrDefault();

            // Adding 1 to return value because 0 is Unknown and 1 is None. If we have a connection we receive at least 1 in return value
            if (!signalStrength.HasValue)
                return SignalStrength.Unknown;
            switch (signalStrength.Value)
            {
                case 0:
                    return SignalStrength.None;
                case 1:
                    return SignalStrength.Weak;
                case 2:
                case 3:
                    return SignalStrength.Fair;
                case 4:
                case 5:
                    return SignalStrength.Strong;
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

        static IEnumerable<ConnectionProfile> PlatformProfiles
        {
            get
            {
                var networkInterfaceList = NetworkInformation.GetConnectionProfiles();
                foreach (var interfaceInfo in networkInterfaceList.Where(nii => nii.GetNetworkConnectivityLevel() != NetworkConnectivityLevel.None))
                {
                    var type = ConnectionProfile.Other;

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
