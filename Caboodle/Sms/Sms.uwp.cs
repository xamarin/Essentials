using System;
using Windows.ApplicationModel.Chat;
using Windows.Devices.Sms;

namespace Microsoft.Caboodle
{
    public static partial class Sms
    {
        static bool GetCanSendSms() => true;

        static bool GetCanSendSmsInBackground() => true;

        private static void PlatformSendSms(string recipient, string message)
        {
            var msg = new ChatMessage { Body = message };
            if (!string.IsNullOrWhiteSpace(recipient))
                msg.Recipients.Add(recipient);

#pragma warning disable 4014
            ChatMessageManager.ShowComposeSmsMessageAsync(msg);
#pragma warning restore 4014
        }

        private static void PlatformSendSmsInBackground(string recipient, string message)
        {
                var sendingMessage = new SmsTextMessage2
                {
                    Body = message,
                    To = recipient
                };

                SmsDevice2.GetDefault().SendMessageAndGetResultAsync(sendingMessage).AsTask().Wait();
        }
    }
}
