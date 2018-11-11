using System;
using System.Collections.Generic;
using NetworkExtension;

namespace Xamarin.Essentials
{
    public static partial class Connectivity
    {
        static ReachabilityListener listener;

        static void StartListeners()
        {
            listener = new ReachabilityListener();
            listener.ReachabilityChanged += OnConnectivityChanged;
        }

        static SignalStrength PlatformSignalStrength()
        {
            var signalstrength = (int)new NEHotspotNetwork().SignalStrength * 3;
            switch (signalstrength)
            {
                case 1:
                    return SignalStrength.Weak;
                case 2:
                    return SignalStrength.Fair;
                case 3:
                    return SignalStrength.Strong;
                default:
                    throw new InvalidOperationException("Exhaustive switch statement hit default value.");
            }
        }

        static void StopListeners()
        {
            if (listener == null)
                return;

            listener.ReachabilityChanged -= OnConnectivityChanged;
            listener.Dispose();
            listener = null;
        }

        static NetworkAccess PlatformNetworkAccess
        {
            get
            {
                var internetStatus = Reachability.InternetConnectionStatus();
                if (internetStatus == NetworkStatus.ReachableViaCarrierDataNetwork || internetStatus == NetworkStatus.ReachableViaWiFiNetwork)
                    return NetworkAccess.Internet;

                var remoteHostStatus = Reachability.RemoteHostStatus();
                if (remoteHostStatus == NetworkStatus.ReachableViaCarrierDataNetwork || remoteHostStatus == NetworkStatus.ReachableViaWiFiNetwork)
                    return NetworkAccess.Internet;

                return NetworkAccess.None;
            }
        }

        static IEnumerable<ConnectionProfile> PlatformProfiles
        {
            get
            {
                var statuses = Reachability.GetActiveConnectionType();
                foreach (var status in statuses)
                {
                    switch (status)
                    {
                        case NetworkStatus.ReachableViaCarrierDataNetwork:
                            yield return ConnectionProfile.Cellular;
                            break;
                        case NetworkStatus.ReachableViaWiFiNetwork:
                            yield return ConnectionProfile.WiFi;
                            break;
                        default:
                            yield return ConnectionProfile.Other;
                            break;
                    }
                }
            }
        }
    }
}
