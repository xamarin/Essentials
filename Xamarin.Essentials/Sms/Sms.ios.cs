﻿using System.Linq;
using System.Threading.Tasks;
using MessageUI;

namespace Xamarin.Essentials
{
    public static partial class Sms
    {
        internal static bool IsComposeSupported
            => MFMessageComposeViewController.CanSendText;

        static Task PlatformComposeAsync(SmsMessage message)
        {
            // do this first so we can throw as early as possible
            var controller = Platform.GetCurrentViewController();

            // create the controller
            var messageController = new MFMessageComposeViewController();
            if (!string.IsNullOrWhiteSpace(message?.Body))
                messageController.Body = message.Body;

            messageController.Recipients = message?.Recipients?.ToArray() ?? new string[] { };

            var tcs = new TaskCompletionSource<bool>();
            messageController.Finished += (sender, e) =>
            {
                messageController.DismissViewController(true, null);
                tcs?.TrySetResult(e.Result == MessageComposeResult.Sent);
            };

            if (controller.PresentationController != null)
            {
                controller.PresentationController.Delegate = new Platform.UIPresentationControllerDelegate
                {
                    DismissHandler = () => tcs.TrySetResult(false)
                };
            }

            controller.PresentViewController(messageController, true, null);

            return tcs.Task;
        }
    }
}
