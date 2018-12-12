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

        static IEnumerable<ConnectionProfile> PlatformConnectionProfiles
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
                            yield return ConnectionProfile.Unknown;
                            break;
                    }
                }
            }
        }

        public static partial class WiFi
        {
            static SignalStrength PlatformSignalStrength()
            {
                var signalStrength = new NEHotspotNetwork().SignalStrength;

                if (signalStrength > .75d)
                    return SignalStrength.Great;

                if (signalStrength > .5d)
                    return SignalStrength.Good;

                if (signalStrength > .25d)
                    return SignalStrength.Moderate;

                if (signalStrength > 0.01d)
                    return SignalStrength.Poor;

                return SignalStrength.None;
            }
        }
    }
}
