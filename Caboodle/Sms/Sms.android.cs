using System;
using System.Threading.Tasks;
using Android.Content;

using Uri = Android.Net.Uri;

namespace Microsoft.Caboodle
{
    public static partial class Sms
    {
        public static bool IsComposeSupported
            => Platform.IsIntentSupported(CreateIntent("0000000000"));

        public static Task ComposeAsync(SmsMessage message)
        {
            var intent = CreateIntent(message);
            Platform.CurrentContext.StartActivity(intent);

            return Task.FromResult(true);
        }

        private static Intent CreateIntent(SmsMessage message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));
            message.Validate();

            return CreateIntent(message.Recipient, message.Body);
        }

        private static Intent CreateIntent(string recipient, string body = null)
        {
            Uri uri;
            if (!string.IsNullOrWhiteSpace(recipient))
                uri = Uri.Parse("smsto:" + recipient);
            else
                uri = Uri.Parse("smsto:");

            var intent = new Intent(Intent.ActionSendto, uri);

            if (!string.IsNullOrWhiteSpace(body))
                intent.PutExtra("sms_body", body);

            return intent;
        }
    }
}
