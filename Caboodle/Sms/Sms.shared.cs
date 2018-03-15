using System;

namespace Microsoft.Caboodle
{
    public static partial class Sms
    {
    }

    public class SmsMessage
    {
        public SmsMessage()
        {
        }

        public SmsMessage(string body, string recipient)
        {
            Body = body;
            Recipient = recipient;
        }

        public string Body { get; set; }

        public string Recipient { get; set; }

        internal void Validate()
        {
            if (string.IsNullOrWhiteSpace(Body))
                throw new ArgumentException("SMS body must not be empty.", nameof(Body));

            if (string.IsNullOrWhiteSpace(Recipient))
                throw new ArgumentException("SMS recipient must not be empty.", nameof(Recipient));
        }
    }
}
