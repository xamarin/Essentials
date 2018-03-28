using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.ApplicationModel.Email;
using Windows.Foundation.Metadata;

namespace Microsoft.Caboodle
{
    public static partial class Email
    {
        internal static bool IsComposeSupported
            => ApiInformation.IsTypePresent("Windows.ApplicationModel.Email.EmailManager");

        static Task PlatformComposeAsync(EmailMessage message)
        {
            if (message != null && message.BodyFormat != EmailBodyFormat.PlainText)
                throw new FeatureNotSupportedException("UWP can only compose plain text email messages.");

            var nativeMessage = new Windows.ApplicationModel.Email.EmailMessage
            {
                Body = message?.Body,
                Subject = message?.Subject,
            };
            Sync(message?.To, nativeMessage.To);
            Sync(message?.Cc, nativeMessage.CC);
            Sync(message?.Bcc, nativeMessage.Bcc);

            return EmailManager.ShowComposeNewEmailAsync(nativeMessage).AsTask();
        }

        static void Sync(List<string> recipients, IList<EmailRecipient> nativeRecipients)
        {
            if (recipients != null)
            {
                foreach (var recipient in recipients)
                {
                    nativeRecipients.Add(new EmailRecipient(recipient));
                }
            }
        }
    }
}
