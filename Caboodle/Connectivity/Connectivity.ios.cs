using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Caboodle
{
	public partial class Connectivity
    {
		public static bool IsConnected
		{
			get
			{
				var remoteHostStatus = Reachability.RemoteHostStatus();
				var internetStatus = Reachability.InternetConnectionStatus();
				
			    return (internetStatus == NetworkStatus.ReachableViaCarrierDataNetwork ||
								internetStatus == NetworkStatus.ReachableViaWiFiNetwork) ||
							  (remoteHostStatus == NetworkStatus.ReachableViaCarrierDataNetwork ||
								remoteHostStatus == NetworkStatus.ReachableViaWiFiNetwork);
			}
		}
    }
}
