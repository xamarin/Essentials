using System;
using System.Linq;
using System.Threading.Tasks;

namespace Xamarin.Essentials
{
    public static partial class Sms
    {
        public static Task ComposeAsync()
            => ComposeAsync(null);

        public static Task ComposeAsync(SmsMessage message)
        {
            if (!IsComposeSupported)
                throw new FeatureNotSupportedException();
            var filteredMessage = new SmsMessage(
                message.Body,
                message.Recipients.Where(x => !string.IsNullOrEmpty(x)).ToArray());
            return PlatformComposeAsync(filteredMessage);
        }
    }

    public class SmsMessage
    {
        public SmsMessage(string body, string[] recipients)
        {
            Body = body;
            Recipients = recipients;
        }

        public string Body { get; set; }

        public string[] Recipients { get; }
    }
}
