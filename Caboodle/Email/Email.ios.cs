using System.IO;
using System.Threading.Tasks;
using Foundation;
using MessageUI;
using UIKit;

namespace Microsoft.Caboodle
{
    public static partial class Email
    {
        public static bool IsEnabled
        {
            get
            {
                return MFMailComposeViewController.CanSendMail;
            }
        }

        public static bool IsAvailable
        {
            get
            {
                return MFMailComposeViewController.CanSendMail;
            }
        }

        /// <summary>
        /// Send Email
        /// </summary>
        /// <returns></returns>
        public static async void Compose(
            string[] recipientsto,
            string[] recipientscc,
            string[] recipientsbcc,
            string subject,
            string body,
            string bodymimetype,
            string[] attachmentspaths)
        {
            Platform.BeginInvokeOnMainThread(() =>
            {
                var vc_mailcompose = new MFMailComposeViewController();

                vc_mailcompose.SetSubject(subject);
                var is_html = false;
                if (bodymimetype == "text/plain")
                {
                    is_html = false;
                }
                else if (bodymimetype == "text/html")
                {
                    is_html = true;
                }
                else
                {
                    throw new EmailException($"Invalid {nameof(bodymimetype)}: {bodymimetype}");
                }

                vc_mailcompose.SetMessageBody(body, is_html);
                vc_mailcompose.SetToRecipients(recipientsto);
                vc_mailcompose.SetCcRecipients(recipientscc);
                vc_mailcompose.SetBccRecipients(recipientsbcc);
                vc_mailcompose.Finished += (s, e) => ((MFMailComposeViewController)s).DismissViewController(true, () => { });

                if (attachmentspaths != null)
                {
                    foreach (var a in attachmentspaths)
                    {
                        vc_mailcompose.AddAttachmentData(NSData.FromFile(a), GetMimeType(a), Path.GetFileName(a));
                    }
                }
                var vc = GetPresentedVewController();
                vc.PresentViewController(vc_mailcompose, true, null);
            });

            return;
        }

        private static UIViewController GetPresentedVewController()
        {
            var vc = UIApplication.SharedApplication.KeyWindow.RootViewController;
            while (vc.PresentedViewController != null)
            {
                vc = vc.PresentedViewController;
            }

            return vc;
        }
    }
}
