﻿using System;
using MessageUI;
using UIKit;

namespace Microsoft.Caboodle
{
    public static partial class Sms
    {
        static bool GetCanSendSms() => MFMessageComposeViewController.CanSendText;

        static bool GetCanSendSmsInBackground() => false;

        public static void PlatformSendSms(string recipient, string message)
        {
            var smsController = new MFMessageComposeViewController() { Body = message };

            if (!string.IsNullOrWhiteSpace(recipient))
            {
                var recipients = recipient.Split(';');
                if (recipients.Length > 0)
                    smsController.Recipients = recipients;
            }

            smsController.Finished += (sender, e) => smsController.DismissViewController(true, null);
            UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(smsController, true, null);

            ////smsController.Body = message;

            ////EventHandler<MFMessageComposeResultEventArgs> handler = null;
            ////handler = (sender, args) =>
            ////   {
            ////       smsController.Finished -= handler;

            ////       if (!(sender is UIViewController uiViewController))
            ////       {
            ////           throw new ArgumentException("sender");
            ////       }

            ////       uiViewController.DismissViewController(true, () => { });
            ////   };

            ////smsController.Finished += handler;

            ////smsController.PresentUsingRootViewController();
        }

        public static void PlatformSendSmsInBackground(string recipient, string message)
        {
            throw new PlatformNotSupportedException("Sending SMS in background not supported on iOS");
        }
    }
}
