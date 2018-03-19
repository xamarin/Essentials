using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.ApplicationModel.Email;
using Windows.Foundation.Metadata;
using Windows.Storage;
using Windows.Storage.Streams;

namespace Microsoft.Caboodle
{
    public static partial class Email
    {
        /// Indicating whether Compose Dialog is available
        public static bool IsComposeSupported
            => ApiInformation.IsTypePresent("Windows.ApplicationModel.Email.EmailMessage");

        /// <summary>
        /// Send Email
        /// </summary>
        /// <returns></returns>
        public static async Task ComposeAsync(
            string[] recipientsto,
            string[] recipientscc,
            string[] recipientsbcc,
            string subject,
            string body,
            string bodymimetype,
            string[] attachmentspaths)
        {
            var emmsg = new Windows.ApplicationModel.Email.EmailMessage
            {
                Subject = subject,
                Body = body,
            };

            recipientsto?.ToList().ForEach(s =>
            {
                emmsg.To.Add(new Windows.ApplicationModel.Email.EmailRecipient(s));
            });

            recipientscc?.ToList().ForEach(s =>
            {
                emmsg.CC.Add(new Windows.ApplicationModel.Email.EmailRecipient(s));
            });

            recipientsbcc?.ToList().ForEach( s =>
            {
                emmsg.Bcc.Add(new Windows.ApplicationModel.Email.EmailRecipient(s));
            });

            attachmentspaths?.ToList().ForEach(async a =>
            {
                var storagefile = await StorageFile.GetFileFromPathAsync(a);
                var stream_reference = RandomAccessStreamReference.CreateFromFile(storagefile);
                emmsg.Attachments.Add(new Windows.ApplicationModel.Email.EmailAttachment(a, stream_reference));
            });

            await EmailManager.ShowComposeNewEmailAsync(emmsg);

            return;
        }
    }
}
