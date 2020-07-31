using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Chat;
using Windows.Devices.Sms;
using Windows.Foundation.Metadata;

namespace Xamarin.Essentials
{
    public static partial class Sms
    {
        internal static bool IsComposeSupported
            => ApiInformation.IsTypePresent("Windows.ApplicationModel.Chat.ChatMessageManager");

        internal static bool IsComposeInBackgroundSupported
            => ApiInformation.IsTypePresent("Windows.ApplicationModel.Chat.ChatMessageManager");

        static Task PlatformComposeAsync(SmsMessage message)
        {
            var chat = new ChatMessage();
            if (!string.IsNullOrWhiteSpace(message?.Body))
                chat.Body = message.Body;

            foreach (var recipient in message?.Recipients)
                chat.Recipients.Add(recipient);

            return ChatMessageManager.ShowComposeSmsMessageAsync(chat).AsTask();
        }

        static Task PlatformComposeInBackgroundAsync(SmsMessage message)
        {
            var sendingMessage = new SmsTextMessage2()
            {
                Body = message.Body,
                To = message.Recipients.First()
            };

            return SmsDevice2.GetDefault().SendMessageAndGetResultAsync(sendingMessage).AsTask();
        }
    }
}
