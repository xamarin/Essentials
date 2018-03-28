using System.Threading.Tasks;
using Foundation;
using MessageUI;

namespace Microsoft.Caboodle
{
    public static partial class Email
    {
        internal static bool IsComposeSupported
            => MFMailComposeViewController.CanSendMail;

        static Task PlatformComposeAsync(EmailMessage message)
        {
            // do this first so we can throw as early as possible
            var parentController = Platform.GetCurrentViewController();

            // create the controller
            var controller = new MFMailComposeViewController();
            if (!string.IsNullOrEmpty(message?.Body))
                controller.SetMessageBody(message.Body, message?.BodyFormat == EmailBodyFormat.Html);
            if (!string.IsNullOrEmpty(message?.Subject))
                controller.SetSubject(message.Subject);
            if (message?.To.Count > 0)
                controller.SetToRecipients(message.To.ToArray());
            if (message?.Cc.Count > 0)
                controller.SetCcRecipients(message.Cc.ToArray());
            if (message?.Bcc.Count > 0)
                controller.SetBccRecipients(message.Bcc.ToArray());

            if (message?.Attachments?.Count > 0)
            {
                foreach (var attachment in message.Attachments)
                {
                    var data = NSData.FromFile(attachment.FilePath);
                    controller.AddAttachmentData(data, attachment.MimeType, attachment.Name);
                }
            }

            // show the controller
            var tcs = new TaskCompletionSource<bool>();
            controller.Finished += (sender, e) =>
            {
                controller.DismissViewController(true, null);
                tcs.SetResult(e.Result == MFMailComposeResult.Sent);
            };
            parentController.PresentViewController(controller, true, null);

            return tcs.Task;
        }
    }
}
