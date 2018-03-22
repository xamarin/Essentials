using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android;
using Android.Content;
using Android.OS;
using Android.Text;
using Android.Net;

namespace Microsoft.Caboodle
{
    public static partial class Email
    {
        /// Indicating whether Compose Dialog is available
        public static bool IsComposeSupported
            => Platform.IsIntentSupported(CreateIntent(
                new EmailMessage()
                {
                    RecipientsTo = new string[] {"mailto:caboodle@mailinator.com" },
                }));

        /// Indicating whether Sending in background is available
        public static bool IsSendSupported
        {
            get;
        }

        private static Intent CreateIntent(EmailMessage message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));
            message.Validate();

            return CreateIntent(message.RecipientsTo, message.RecipientsCC, message.RecipientsBCC, message.Subject, message.Body);
        }

        private static Intent CreateIntent(
            string[] recipientsto,
            string[] recipientscc,
            string[] recipientsbcc,
            string subject = null,
            string body = null,
            string bodymimetype = null,
            string[] attachmentspaths = null
            )
        {
            var uri = Android.Net.Uri.Parse("mailto:");

            // TODO: Tuning for Intent Choosers/Pickers
            // Numerous apps can respond to ActionSend
            var i = new Intent(Intent.ActionSend, uri);
            i.SetData(uri);
            i.SetType("plain/text");    // i.SetType("message/rfc822");

            if (! (recipientsto?.Length > 0))
                i.PutExtra(Intent.ExtraEmail, recipientsto);

            if (!(recipientsto?.Length > 0))
                i.PutExtra(Intent.ExtraCc, recipientsto);

            if (!(recipientsto?.Length > 0))
                i.PutExtra(Intent.ExtraBcc, recipientsto);

            if (!string.IsNullOrWhiteSpace(subject))
                i.PutExtra(Intent.ExtraSubject, subject);

            if (!string.IsNullOrWhiteSpace(body))
            {
                if (bodymimetype == "text/plain")
                {
                    i.PutExtra(Intent.ExtraText, body);
                }
                else if (bodymimetype == "text/html")
                {
                    i.PutExtra(Intent.ExtraHtmlText, Html.FromHtml(body, FromHtmlOptions.ModeLegacy));
                }
                else
                {
                    throw new EmailException($"Invalid {nameof(bodymimetype)}: {bodymimetype}");
                }
                i.SetType(bodymimetype);
            }

            if (attachmentspaths != null)
            {
                i.AddAttachments(attachmentspaths);
            }

            return i;
        }

        /// <summary>
        /// Compose Email
        /// </summary>
        /// <returns></returns>
        /// <see cref="http://developer.xamarin.com/recipes/android/networking/email/send_an_email/"/>
        /// <see cref="http://stackoverflow.com/questions/15946297/sending-email-with-attachment-using-sendto-on-some-devices-doesnt-work"/>
        public static async Task ComposeAsync(
            string[] recipientsto,
            string[] recipientscc,
            string[] recipientsbcc,
            string subject,
            string body,
            string bodymimetype,
            string[] attachmentspaths)
        {
            var intent = CreateIntent(
                recipientsto,
                recipientscc,
                recipientsbcc,
                subject,
                body,
                bodymimetype,
                attachmentspaths);

            var chooser = Intent.CreateChooser(intent, "Send Mail");
            Platform.CurrentActivity.StartActivity(chooser);
            //Platform.CurrentActivity.StartActivity(intent);

            return;
        }
    }

    public static class EmailExtensionsAndroid
    {
        public static void AddAttachments(this Intent i, IEnumerable<string> attachments_as_files)
        {
            if (attachments_as_files == null || !attachments_as_files.Any())
            {
                return;
            }

            var uris_for_files = new List<IParcelable>();
            foreach (var a in attachments_as_files)
            {
                var file = new Java.IO.File(a);
                if (file.Exists())
                {
                    var u = Android.Net.Uri.FromFile(file);
                    uris_for_files.Add(u);
                }
                else
                {
                    throw new EmailException($"Attachment File does not exist: {a}");
                }
            }

            i.PutParcelableArrayListExtra(Intent.ExtraStream, uris_for_files);

            return;
        }
    }
}
