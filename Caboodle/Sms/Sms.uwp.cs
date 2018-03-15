using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Chat;
using Windows.Foundation.Metadata;

namespace Microsoft.Caboodle
{
    public static partial class Sms
    {
        public static bool IsComposeSupported
            => ApiInformation.IsTypePresent("Windows.ApplicationModel.Chat.ChatMessageManager");

        public static Task ComposeAsync(SmsMessage message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));
            message.Validate();

            var chat = new ChatMessage
            {
                Body = message.Body,
                Recipients =
                {
                    message.Recipient
                }
            };

            return ChatMessageManager.ShowComposeSmsMessageAsync(chat).AsTask();
        }
    }
}
