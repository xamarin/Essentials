using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Caboodle
{
    public partial class Connectivity
    {
    }

	public enum ConnectionType
	{
		Bluetooth,
		Cellular,
		Ethernet,
		WiMAX,
		WiFi,
		Other
	}
	
	public class ConnectivityChangedEventArgs : EventArgs
	{
		public bool IsConnected { get; set; }
	}
	
	public class ConnectivityTypeChangedEventArgs : EventArgs
	{
		public IEnumerable<ConnectionType> ConnectionTypes { get; set; }
	}
	
	public delegate void ConnectivityChangedEventHandler(object sender, ConnectivityChangedEventArgs e);
	
	public delegate void ConnectivityTypeChangedEventHandler(object sender, ConnectivityTypeChangedEventArgs e);
}
