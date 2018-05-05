using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Foundation;
using MessageUI;
using UIKit;

namespace Xamarin.Essentials
{
    public static partial class Email
    {
        internal static bool IsComposeSupported
        {
            get
            {
                var can = MFMailComposeViewController.CanSendMail;
                if (!can)
                {
                    var url = NSUrl.FromString("mailto://?to=test@xamarin.com");
                    NSRunLoop.Main.InvokeOnMainThread(() => can = UIApplication.SharedApplication.CanOpenUrl(url));
                }
                return can;
            }
        }

        static Task PlatformComposeAsync(EmailMessage message)
        {
            if (MFMailComposeViewController.CanSendMail)
                return ComposeWithMailCompose(message);
            else
                return ComposeWithUrl(message);
        }

        static Task ComposeWithMailCompose(EmailMessage message)
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

        static Task ComposeWithUrl(EmailMessage message)
        {
            if (message != null && message.BodyFormat != EmailBodyFormat.PlainText)
                throw new FeatureNotSupportedException("iOS can only compose plain text email messages when a default email account is not set up.");

            var parts = new List<string>();
            if (!string.IsNullOrEmpty(message?.Body))
                parts.Add("body=" + Uri.EscapeUriString(message.Body));
            if (!string.IsNullOrEmpty(message?.Subject))
                parts.Add("subject=" + Uri.EscapeUriString(message.Subject));
            if (message?.To.Count > 0)
                parts.Add("to=" + string.Join(",", message.To));
            if (message?.Cc.Count > 0)
                parts.Add("cc=" + string.Join(",", message.Cc));
            if (message?.Bcc.Count > 0)
                parts.Add("bcc=" + string.Join(",", message.Bcc));

            var url = "mailto://?" + string.Join("&", parts);

            var tcs = new TaskCompletionSource<bool>();
            NSRunLoop.Main.InvokeOnMainThread(() =>
            {
                var nsurl = NSUrl.FromString(url);
                UIApplication.SharedApplication.OpenUrl(nsurl, (NSDictionary)null, r => tcs.TrySetResult(r));
            });
            return tcs.Task;
        }
    }
}
