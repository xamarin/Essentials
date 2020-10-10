using System;
using System.Linq;
using System.Threading.Tasks;

namespace Xamarin.Essentials
{
    public static partial class Sms
    {
        internal static bool IsComposeSupported
            => true;

        static Task PlatformComposeAsync(SmsMessage message)
        {
            return Launcher.OpenAsync("sms:" + CombineRecipients(message.Recipients) + (string.IsNullOrEmpty(message.Body) ? string.Empty : "?body=" + Uri.EscapeUriString(message.Body)));
        }

        static string CombineRecipients(System.Collections.Generic.List<string> recipients)
        {
            if (recipients == null)
                return string.Empty;

            var stringBuilder = new System.Text.StringBuilder();

            foreach (var recipient in recipients)
            {
                stringBuilder.Append(recipient + ",");
            }

            if (stringBuilder.Length > 0)
                stringBuilder.Length -= 1;

            return stringBuilder.ToString();
        }
    }
}
