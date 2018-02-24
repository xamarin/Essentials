using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Caboodle
{
	public partial class Connectivity
    {
		public static NetworkAccess NetworkAccess
        {
			get
			{
				var remoteHostStatus = Reachability.RemoteHostStatus();
				var internetStatus = Reachability.InternetConnectionStatus();
				
			    var isConnected = (internetStatus == NetworkStatus.ReachableViaCarrierDataNetwork ||
								internetStatus == NetworkStatus.ReachableViaWiFiNetwork) ||
							  (remoteHostStatus == NetworkStatus.ReachableViaCarrierDataNetwork ||
								remoteHostStatus == NetworkStatus.ReachableViaWiFiNetwork);

                return isConnected ? NetworkAccess.Internet : NetworkAccess.None;
            }
		}
    }
}
