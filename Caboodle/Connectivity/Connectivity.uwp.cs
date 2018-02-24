using System;
using System.Collections.Generic;
using System.Text;
using Windows.Networking.Connectivity;

namespace Microsoft.Caboodle
{
	public partial class Connectivity
    {
		public static bool IsConnected
		{
			get
			{
				var profile = NetworkInformation.GetInternetConnectionProfile();
				if (profile == null)
					return false;

				var level = profile.GetNetworkConnectivityLevel();
				return level != NetworkConnectivityLevel.None && level != NetworkConnectivityLevel.LocalAccess;
			}
		}
	}
}
