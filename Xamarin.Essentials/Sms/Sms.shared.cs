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

            return PlatformComposeAsync(message);
        }
    }

    public class SmsMessage
    {
        public SmsMessage(string body, string[] recipients)
        {
            Body = body;
            Recipients = recipients.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
        }

        public string Body { get; set; }

        public string[] Recipients { get; }
    }
}
