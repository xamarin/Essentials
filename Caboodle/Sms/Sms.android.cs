using Android.App;
using Android.Content;
using Android.Net;
using Android.Telephony;

namespace Microsoft.Caboodle
{
    public static partial class Sms
    {
        static bool GetCanSendSms() => true;

        static bool GetCanSendSmsInBackground() => true;

        public static void PlatformSendSms(string recipient, string message)
        {
            Uri smsUri;
            if (!string.IsNullOrWhiteSpace(recipient))
                smsUri = Uri.Parse("smsto:" + recipient);
            else
                smsUri = Uri.Parse("smsto:");

            var smsIntent = new Intent(Intent.ActionSendto, smsUri);
            smsIntent.PutExtra("sms_body", message);
            Platform.CurrentContext.StartActivity(smsIntent);
        }

        public static void PlatformSendSmsInBackground(string recipient, string message)
        {
            var smsManager = SmsManager.Default;
            smsManager.SendTextMessage(recipient, null, message, null, null);
        }
    }
}
