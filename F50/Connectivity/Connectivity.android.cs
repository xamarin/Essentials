using Android.App;
using Android.Content;
using Android.Net;
using Android.Net.Wifi;
using System;
using System.Collections.Generic;
using System.Text;

namespace Xamarin.F50
{
    public partial class Connectivity
    {

		static ConnectivityManager connectivityManager;
		static WifiManager wifiManager;

		static ConnectivityManager ConnectivityManager
		{
			get
			{
				if (connectivityManager == null || connectivityManager.Handle == IntPtr.Zero)
					connectivityManager = (ConnectivityManager)(Application.Context.GetSystemService(Context.ConnectivityService));

				return connectivityManager;
			}
		}

		static WifiManager WifiManager
		{
			get
			{
				if (wifiManager == null || wifiManager.Handle == IntPtr.Zero)
					wifiManager = (WifiManager)(Application.Context.GetSystemService(Context.WifiService));

				return wifiManager;
			}
		}

		public static bool IsConnected
		{
			get
			{
				try
				{
					var manager = ConnectivityManager;

					//When on API 21+ need to use getAllNetworks, else fall base to GetAllNetworkInfo
					//https://developer.android.com/reference/android/net/ConnectivityManager.html#getAllNetworks()
					if ((int)global::Android.OS.Build.VERSION.SdkInt >= 21)
					{
						foreach (var network in manager.GetAllNetworks())
						{
							try
							{
								var capabilities = manager.GetNetworkCapabilities(network);

								if (capabilities == null)
									continue;

								//check to see if it has the internet capability
								if (!capabilities.HasCapability(NetCapability.Internet))
									continue;

								var info = manager.GetNetworkInfo(network);

								if (info == null || !info.IsAvailable)
									continue;

								if (info.IsConnected)
									return true;
							}
							catch
							{
								//there is a possibility, but don't worry
							}
						}
					}
					else
					{
#pragma warning disable CS0618 // Type or member is obsolete
						foreach (var info in manager.GetAllNetworkInfo())
#pragma warning restore CS0618 // Type or member is obsolete
						{
							if (info == null || !info.IsAvailable)
								continue;

							if (info.IsConnected)
								return true;
						}
					}

					return false;
				}
				catch (Exception e)
				{
					Console.WriteLine("Unable to get connected state - do you have ACCESS_NETWORK_STATE permission? - error: {0}", e);
					return false;
				}
			}
		}
    }
}
