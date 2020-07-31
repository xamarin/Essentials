using System.Threading.Tasks;
using Tizen.Applications;

namespace Xamarin.Essentials
{
    public static partial class Sms
    {
        internal static bool IsComposeSupported
            => Platform.GetFeatureInfo<bool>("network.telephony.sms");

        internal static bool IsComposeInBackgroundSupported
            => throw ExceptionUtils.NotSupportedOrImplementedException;

        static Task PlatformComposeAsync(SmsMessage message)
        {
            Permissions.EnsureDeclared<Permissions.LaunchApp>();

            var appControl = new AppControl
            {
                Operation = AppControlOperations.Compose,
                Uri = "sms:",
            };

            if (!string.IsNullOrEmpty(message.Body))
                appControl.ExtraData.Add("http://tizen.org/appcontrol/data/text", message.Body);
            if (message.Recipients.Count > 0)
                appControl.Uri += string.Join(" ", message.Recipients);

            AppControl.SendLaunchRequest(appControl);

            return Task.CompletedTask;
        }

        static Task PlatformComposeInBackgroundAsync(SmsMessage message)
            => throw ExceptionUtils.NotSupportedOrImplementedException;
    }
}
