using System;

namespace Microsoft.Caboodle
{
    public static partial class Sms
    {
        public static bool CanSendSms => GetCanSendSms();

        public static bool CanSendSmsInBackground => GetCanSendSmsInBackground();

        public static void SendSms(string recipient, string message = null, SmsSendType sendType = SmsSendType.Foreground)
        {
            if (recipient == null)
                throw new ArgumentNullException(nameof(recipient));

            if (message == null)
                throw new ArgumentNullException(nameof(message));

            switch (sendType)
            {
                case SmsSendType.Foreground:
                    PlatformSendSms(recipient, message);
                    break;
                case SmsSendType.Background:
                    PlatformSendSmsInBackground(recipient, message);
                    break;
                case SmsSendType.PreferBackground:
                    if (CanSendSmsInBackground)
                        PlatformSendSmsInBackground(recipient, message);
                    else
                        PlatformSendSms(recipient, message);
                    break;
                default:
                    break;
            }
        }
    }

    public enum SmsSendType
    {
        Foreground,
        Background,
        PreferBackground
    }
}
