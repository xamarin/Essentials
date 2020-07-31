using System.Collections.Generic;
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

            FillMessageIfAnyMandatoryFieldsIsNull(message);

            return PlatformComposeAsync(message);
        }

        public static Task ComposeInBackgroundAsync(SmsMessage message)
        {
            if (!IsComposeInBackgroundSupported)
                throw new FeatureNotSupportedException();

            FillMessageIfAnyMandatoryFieldsIsNull(message);

            return PlatformComposeInBackgroundAsync(message);
        }

        static void FillMessageIfAnyMandatoryFieldsIsNull(SmsMessage message)
        {
            if (message == null)
                message = new SmsMessage();

            if (message?.Recipients == null)
                message.Recipients = new List<string>();
        }
    }

    public class SmsMessage
    {
        public SmsMessage()
        {
        }

        public SmsMessage(string body, string recipient)
        {
            Body = body;
            if (!string.IsNullOrWhiteSpace(recipient))
                Recipients.Add(recipient);
        }

        public SmsMessage(string body, IEnumerable<string> recipients)
        {
            Body = body;
            if (recipients != null)
            {
                Recipients.AddRange(recipients.Where(x => !string.IsNullOrWhiteSpace(x)));
            }
        }

        public string Body { get; set; }

        public List<string> Recipients { get; set; } = new List<string>();
    }
}
