using System;
using System.Threading.Tasks;
using MessageUI;

namespace Microsoft.Caboodle
{
    public static partial class Sms
    {
        public static bool IsComposeSupported
            => MFMessageComposeViewController.CanSendText;

        public static Task ComposeAsync(SmsMessage message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));
            message.Validate();

            // do this first so we can throw as early as possible
            var controller = Platform.GetCurrentViewController();

            // show the controller
            var tcs = new TaskCompletionSource<bool>();
            var messageController = new MFMessageComposeViewController
            {
                Body = message.Body,
                Recipients = new[] { message.Recipient }
            };
            messageController.Finished += (sender, e) =>
            {
                messageController.DismissViewController(true, null);
                tcs.SetResult(e.Result == MessageComposeResult.Sent);
            };
            controller.PresentViewController(messageController, true, null);

            return tcs.Task;
        }
    }
}
