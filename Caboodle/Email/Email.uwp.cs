using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.ApplicationModel.Email;
using Windows.Foundation.Metadata;
using Windows.Storage;
using Windows.Storage.Streams;

using NativeEmailAttachment = Windows.ApplicationModel.Email.EmailAttachment;
using NativeEmailMessage = Windows.ApplicationModel.Email.EmailMessage;

namespace Microsoft.Caboodle
{
    public static partial class Email
    {
        internal static bool IsComposeSupported
            => ApiInformation.IsTypePresent("Windows.ApplicationModel.Email.EmailManager");

        static async Task PlatformComposeAsync(EmailMessage message)
        {
            if (message != null && message.BodyFormat != EmailBodyFormat.PlainText)
                throw new FeatureNotSupportedException("UWP can only compose plain text email messages.");

            var nativeMessage = new NativeEmailMessage
            {
                Body = message?.Body,
                Subject = message?.Subject,
            };
            Sync(message?.To, nativeMessage.To);
            Sync(message?.Cc, nativeMessage.CC);
            Sync(message?.Bcc, nativeMessage.Bcc);

            if (message?.Attachments?.Count > 0)
            {
                foreach (var attachment in message.Attachments)
                {
                    var file = await StorageFile.GetFileFromPathAsync(attachment.FilePath);
                    var data = RandomAccessStreamReference.CreateFromFile(file);
                    var nativeAttachment = new NativeEmailAttachment(attachment.Name, data);
                    if (!string.IsNullOrWhiteSpace(attachment.MimeType))
                        nativeAttachment.MimeType = attachment.MimeType;
                    nativeMessage.Attachments.Add(nativeAttachment);
                }
            }

            await EmailManager.ShowComposeNewEmailAsync(nativeMessage);
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
