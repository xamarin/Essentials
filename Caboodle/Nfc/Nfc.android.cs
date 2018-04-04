using System;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Nfc;
using Android.OS;

namespace Microsoft.Caboodle
{
    public static partial class Nfc
    {
        static NfcAdapter nfcAdapter;

        internal static bool IsSupported
            => Platform.HasSystemFeature(PackageManager.FeatureNfc);

        static void StartTagListeners()
        {
            Ensure();

            var activity = Platform.CurrentActivity;

            var intent = new Intent(activity, activity.GetType())
                .AddFlags(ActivityFlags.SingleTop);
            var pendingIntent = PendingIntent.GetActivity(activity, 0, intent, 0);

            var tagDetected = new IntentFilter(NfcAdapter.ActionTagDiscovered);
            var filters = new[] { tagDetected };

            nfcAdapter.EnableForegroundDispatch(activity, pendingIntent, filters, null);
        }

        static void StopTagListeners()
        {
        }

        static void StartNdefMessageListeners()
        {
            Ensure();
        }

        static void StopNdefMessageListeners()
        {
        }

        static void PlatformPublishMessage(string messageType, byte[] message)
        {
            Ensure();
        }

        static void PlatformStopPublishing()
        {
        }

        static void Ensure()
        {
            // Permissions.RequireAsync(PermissionType.Flashlight);

            nfcAdapter = NfcAdapter.GetDefaultAdapter(Platform.CurrentContext);

            if (nfcAdapter?.IsEnabled != true)
                throw new NotSupportedException();
        }
    }
}
