using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Telephony;
using Uri = Android.Net.Uri;
using Java.Util;

namespace Microsoft.Caboodle
{
	public static partial class PhoneDialer
	{
		public static bool IsSupported
		{
			get
			{
				var packageManager = Application.Context.PackageManager;
				var dialIntent = ResolveDialIntent("0000000000");
				return dialIntent.ResolveActivity(packageManager) != null;
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
			
			string phoneNumber;
			if (Build.VERSION.SdkInt >= BuildVersionCodes.N)
			{
				phoneNumber = PhoneNumberUtils.FormatNumber(number, Locale.GetDefault(Locale.Category.Format).Country);
			}
			else if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
			{
				phoneNumber = PhoneNumberUtils.FormatNumber(number, Locale.Default.Country);
			}
			else
			{
#pragma warning disable CS0618
				phoneNumber = PhoneNumberUtils.FormatNumber(number);
#pragma warning restore CS0618
			}
			
			var dialIntent = ResolveDialIntent(phoneNumber);
			dialIntent.StartNewActivity();
		}

		private static Intent ResolveDialIntent(string number)
		{
			var telUri = Uri.Parse($"tel:{number}");
			return new Intent(Intent.ActionDial, telUri);
		}
	}
}
