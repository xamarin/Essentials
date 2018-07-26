using System.Linq;
using System.Threading.Tasks;
using Android.Content;
using Android.OS;
using Android.Provider;

using AndroidUri = Android.Net.Uri;

namespace Xamarin.Essentials
{
    public static partial class Sms
    {
        internal static bool IsComposeSupported
            => Platform.IsIntentSupported(CreateIntent(new[] { "0000000000" }));

        static Task PlatformComposeAsync(SmsMessage message)
        {
            var intent = CreateIntent(message)
                .SetFlags(ActivityFlags.ClearTop)
                .SetFlags(ActivityFlags.NewTask);

            Platform.AppContext.StartActivity(intent);

            return Task.FromResult(true);
        }

        static Intent CreateIntent(SmsMessage message)
            => CreateIntent(message?.Recipients, message?.Body);

        static Intent CreateIntent(string[] recipients, string body = null)
        {
            Intent intent = null;

            body = body ?? string.Empty;

            if (recipients.All(x => string.IsNullOrWhiteSpace(x)) && Platform.HasApiLevel(BuildVersionCodes.Kitkat))
            {
                var packageName = Telephony.Sms.GetDefaultSmsPackage(Platform.AppContext);
                if (!string.IsNullOrWhiteSpace(packageName))
                {
                    intent = new Intent(Intent.ActionSend);
                    intent.SetType("text/plain");
                    intent.PutExtra(Intent.ExtraText, body);
                    intent.SetPackage(packageName);

                    return intent;
                }
            }

            // Fall back to normal send
            intent = new Intent(Intent.ActionView);

            if (!string.IsNullOrWhiteSpace(body))
                intent.PutExtra("sms_body", body);

            var recipienturi = string.Join(";", recipients.Select(r => AndroidUri.Encode(r)));

            var uri = AndroidUri.Parse($"smsto:{recipienturi}");
            intent.SetData(uri);

            return intent;
        }
    }
}
