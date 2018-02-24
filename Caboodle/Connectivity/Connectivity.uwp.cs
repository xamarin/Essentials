using System;
using System.Collections.Generic;
using System.Text;
using Windows.Networking.Connectivity;

namespace Microsoft.Caboodle
{
	public partial class Connectivity
    {
		public static NetworkAccess NetworkAccess
		{
			get
			{
				var profile = NetworkInformation.GetInternetConnectionProfile();
				if (profile == null)
					return NetworkAccess.Unknown;

				var level = profile.GetNetworkConnectivityLevel();
				switch(level)
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
	}
}
