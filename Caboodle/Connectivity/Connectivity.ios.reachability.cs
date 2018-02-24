using System;
using System.Net;
using CoreFoundation;
using System.Diagnostics;
using SystemConfiguration;
using System.Threading.Tasks;
using System.Collections.Generic;


namespace Microsoft.Caboodle
{

	/// <summary>
	/// Status of network enum
	/// </summary>
	[Foundation.Preserve(AllMembers = true)]
	internal enum NetworkStatus
	{
		/// <summary>
		/// No internet connection
		/// </summary>
		NotReachable,
		/// <summary>
		/// Reachable view Cellular.
		/// </summary>
		ReachableViaCarrierDataNetwork,
		/// <summary>
		/// Reachable view wifi
		/// </summary>
		ReachableViaWiFiNetwork
	}

	/// <summary>
	/// Reachability helper
	/// </summary>
	[Foundation.Preserve(AllMembers = true)]
    internal static class Reachability
    {


        /// <summary>
        /// Default host name to use
        /// </summary>
        internal static string HostName = "www.microsoft.com";

        /// <summary>
        /// Checks if reachable without requiring a connection
        /// </summary>
        /// <param name="flags"></param>
        /// <returns></returns>
        internal static bool IsReachableWithoutRequiringConnection(NetworkReachabilityFlags flags)
        {
            // Is it reachable with the current network configuration?
            var isReachable = (flags & NetworkReachabilityFlags.Reachable) != 0;

            // Do we need a connection to reach it?
            var noConnectionRequired = (flags & NetworkReachabilityFlags.ConnectionRequired) == 0;

            // Since the network stack will automatically try to get the WAN up,
            // probe that
            if ((flags & NetworkReachabilityFlags.IsWWAN) != 0)
                noConnectionRequired = true;

            return isReachable && noConnectionRequired;
        }

        /// <summary>
        /// Checks if host is reachable
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        internal static bool IsHostReachable(string host, int port)
        {
            if (string.IsNullOrWhiteSpace(host))
                return false;

            IPAddress address;
            if (!IPAddress.TryParse(host + ":" + port, out address))
            {
                Debug.WriteLine(host + ":" + port + " is not valid");
                return false;
            }
            using (var r = new NetworkReachability(host))
            {

                NetworkReachabilityFlags flags;

                if (r.TryGetFlags(out flags))
                {
                    return IsReachableWithoutRequiringConnection(flags);
                }
            }
            return false;
        }

        /// <summary>
        ///  Is the host reachable with the current network configuration
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        internal static bool IsHostReachable(string host)
        {
            if (string.IsNullOrWhiteSpace(host))
                return false;

            using (var r = new NetworkReachability(host))
            {

                NetworkReachabilityFlags flags;

                if (r.TryGetFlags(out flags))
                {
                    return IsReachableWithoutRequiringConnection(flags);
                }
            }
            return false;
        }


        /// <summary>
        /// Raised every time there is an interesting reachable event,
        /// we do not even pass the info as to what changed, and
        /// we lump all three status we probe into one
        /// </summary>
        internal static event EventHandler ReachabilityChanged;

        static async void OnChange(NetworkReachabilityFlags flags)
        {
            await Task.Delay(100);
            ReachabilityChanged?.Invoke(null, EventArgs.Empty);

        }
	


        static NetworkReachability defaultRouteReachability;
        static bool IsNetworkAvailable(out NetworkReachabilityFlags flags)
        {

            if (defaultRouteReachability == null)
            {
                var ip = new IPAddress(0);
                defaultRouteReachability = new NetworkReachability(ip);
                defaultRouteReachability.SetNotification(OnChange);
                defaultRouteReachability.Schedule(CFRunLoop.Main, CFRunLoop.ModeDefault);
            }
            if (!defaultRouteReachability.TryGetFlags(out flags))
                return false;
            return IsReachableWithoutRequiringConnection(flags);
        }

        static NetworkReachability remoteHostReachability;
        /// <summary>
        /// Checks the remote host status
        /// </summary>
        /// <returns></returns>
        internal static NetworkStatus RemoteHostStatus()
        {
            NetworkReachabilityFlags flags;
            bool reachable;

            if (remoteHostReachability == null)
            {
                remoteHostReachability = new NetworkReachability(HostName);

                // Need to probe before we queue, or we wont get any meaningful values
                // this only happens when you create NetworkReachability from a hostname
                reachable = remoteHostReachability.TryGetFlags(out flags);

                remoteHostReachability.SetNotification(OnChange);
                remoteHostReachability.Schedule(CFRunLoop.Main, CFRunLoop.ModeDefault);
            }
            else
                reachable = remoteHostReachability.TryGetFlags(out flags);

            if (!reachable)
                return NetworkStatus.NotReachable;

            if (!IsReachableWithoutRequiringConnection(flags))
                return NetworkStatus.NotReachable;

            if ((flags & NetworkReachabilityFlags.IsWWAN) != 0)
                return NetworkStatus.ReachableViaCarrierDataNetwork;


            return NetworkStatus.ReachableViaWiFiNetwork;
        }

        /// <summary>
        /// Checks internet connection status
        /// </summary>
        /// <returns></returns>
        internal static IEnumerable<NetworkStatus> GetActiveConnectionType()
        {
            var status = new List<NetworkStatus>();

			var defaultNetworkAvailable = IsNetworkAvailable(out var flags);

			// If it's a WWAN connection..
			if ((flags & NetworkReachabilityFlags.IsWWAN) != 0)
			{
				status.Add(NetworkStatus.ReachableViaCarrierDataNetwork);
			}
			else if (defaultNetworkAvailable)
			{
				status.Add(NetworkStatus.ReachableViaWiFiNetwork);
			}
			else if (((flags & NetworkReachabilityFlags.ConnectionOnDemand) != 0
				|| (flags & NetworkReachabilityFlags.ConnectionOnTraffic) != 0)
				&& (flags & NetworkReachabilityFlags.InterventionRequired) == 0)
			{
				// If the connection is on-demand or on-traffic and no user intervention
				// is required, then assume WiFi.
				status.Add(NetworkStatus.ReachableViaWiFiNetwork);
			}
			
            return status;
        }

        /// <summary>
        /// Checks internet connection status
        /// </summary>
        /// <returns></returns>
        public static NetworkStatus InternetConnectionStatus()
        {
            var status = NetworkStatus.NotReachable;

			var defaultNetworkAvailable = IsNetworkAvailable(out var flags);

			// If it's a WWAN connection..
			if ((flags & NetworkReachabilityFlags.IsWWAN) != 0)
				status = NetworkStatus.ReachableViaCarrierDataNetwork;

            // If the connection is reachable and no connection is required, then assume it's WiFi
            if (defaultNetworkAvailable)
            {
                status = NetworkStatus.ReachableViaWiFiNetwork;
            }

            // If the connection is on-demand or on-traffic and no user intervention
            // is required, then assume WiFi.
            if (((flags & NetworkReachabilityFlags.ConnectionOnDemand) != 0
                || (flags & NetworkReachabilityFlags.ConnectionOnTraffic) != 0)
                && (flags & NetworkReachabilityFlags.InterventionRequired) == 0)
            {
                status = NetworkStatus.ReachableViaWiFiNetwork;
            }


            return status;
        }

        /// <summary>
        /// Dispose
        /// </summary>
        internal static void Dispose()
        {
            if (remoteHostReachability != null)
            {
                remoteHostReachability.Dispose();
                remoteHostReachability = null;
            }

            if (defaultRouteReachability != null)
            {
                defaultRouteReachability.Dispose();
                defaultRouteReachability = null;
            }
        }

    }
}
