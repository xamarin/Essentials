using System;
using Foundation;
using UIKit;
using CoreTelephony;

namespace Microsoft.Caboodle
{
	public static partial class PhoneDialer
	{
		public static bool IsSupported
		{
			get
			{
				var nsUrl = CreateNsUrl("0000000000");
				var canCall = UIApplication.SharedApplication.CanOpenUrl(nsUrl);
				
				if (canCall)
				{
					using (var netInfo = new CTTelephonyNetworkInfo())
					{
						var mnc = netInfo.SubscriberCellularProvider?.MobileNetworkCode;
						return !string.IsNullOrEmpty(mnc) && mnc != "65535"; // 65535 stands for NoNetwordProvider
					}
				}
				
				return false;
			}
		}

		public static void Open(string number)
		{
			if (string.IsNullOrWhiteSpace(number))
			{
				throw new ArgumentNullException(nameof(number));
			}

			if (!IsSupported)
			{
				throw new CapabilityNotSupportedException();
			}
			
			var nsUrl = CreateNsUrl(number);
			UIApplication.SharedApplication.OpenUrl(nsUrl);
		}

		private NSUrl CreateNsUrl(string number)
		{
			return new NSUrl(new Uri($"tel:{number}").AbsoluteUri);
		}
	}
}
