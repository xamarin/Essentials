using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android;
using Android.Content;
using Android.OS;
using Android.Text;

namespace Microsoft.Caboodle
{
    public static partial class Email
    {
        public static bool IsEnabled
        {
            get
            {
                return true;
            }
        }

        public static bool IsAvailable
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Send Email
        /// </summary>
        /// <returns></returns>
        /// <see cref="http://developer.xamarin.com/recipes/android/networking/email/send_an_email/"/>
        /// <see cref="http://stackoverflow.com/questions/15946297/sending-email-with-attachment-using-sendto-on-some-devices-doesnt-work"/>
        public static void Compose(
            string[] recipientsto,
            string[] recipientscc,
            string[] recipientsbcc,
            string subject,
            string body,
            string bodymimetype,
            string[] attachmentspaths)
        {
            // TODO: Tuning for Intent Choosers/Pickers
            // Numerous apps can respond to ActionSend
            var i = new Intent(Intent.ActionSendto);
            i.SetType("message/rfc822");
            i.SetData(Android.Net.Uri.Parse("mailto"));

            i.PutExtra(Intent.ExtraSubject, subject);

            if (bodymimetype == "text/plain")
            {
                i.PutExtra(Intent.ExtraText, body);
            }
            else if (bodymimetype == "text/html")
            {
                i.PutExtra(Intent.ExtraText, Html.FromHtml(body, FromHtmlOptions.ModeLegacy));
            }
            else
            {
                throw new EmailException($"Invalid {nameof(bodymimetype)}: {bodymimetype}");
            }
            i.SetType(bodymimetype);

            if (attachmentspaths != null)
            {
                i.AddAttachments(attachmentspaths);
            }
            i.PutExtra(Intent.ExtraEmail, recipientsto);
            i.PutExtra(Intent.ExtraCc, recipientscc);
            i.PutExtra(Intent.ExtraBcc, recipientsbcc);

            Platform.CurrentActivity.StartActivity(i);

            // var chooser = i.createChooser(i, "Send Mail");
            // Platform.CurrentActivity.StartActivity(chooser);

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
