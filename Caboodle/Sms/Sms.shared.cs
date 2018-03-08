using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Caboodle
{
    public static partial class Sms
    {
        public static bool CanSendSms => GetCanSendSms();

        public static bool CanSendSmsInBackground => GetCanSendSmsInBackground();

        public static void SendSms(string recipient = null, string message = null)
        {
            if (recipient == null)
                throw new ArgumentNullException(nameof(recipient));

            if (message == null)
                throw new ArgumentNullException(nameof(message));

            if (CanSendSms)
                PlatformSendSms(recipient, message);
        }

        public static void SendSmsInBackground(string recipient, string message = null)
        {
            if (recipient == null)
                throw new ArgumentNullException(nameof(recipient));

            if (message == null)
                throw new ArgumentNullException(nameof(message));

            if (CanSendSmsInBackground)
            {
                PlatformSendSmsInBackground(recipient, message);
            }
        }
    }
}
