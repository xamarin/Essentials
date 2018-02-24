using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Caboodle
{
    public partial class Connectivity
    {
    }

	public enum ConnectionProfile
	{
		Bluetooth,
		Cellular,
		Ethernet,
		WiMAX,
		WiFi,
		Other
	}


	public enum NetworkAccess
    {
        ConstrainedInternet,
        Internet,
        Local,
        None,
        Unknown
    }
	
	public class ConnectivityChangedEventArgs : EventArgs
	{
		public NetworkAccess NetworkAccess { get; set; }
	}
	
	public class ConnectivityProfileChangedEventArgs : EventArgs
	{
		public IEnumerable<ConnectionProfile> Profiles { get; set; }
	}
	
	public delegate void ConnectivityChangedEventHandler(object sender, ConnectivityChangedEventArgs e);
	
	public delegate void ConnectivityProfileChangedEventHandler(object sender, ConnectivityProfileChangedEventArgs e);
}
