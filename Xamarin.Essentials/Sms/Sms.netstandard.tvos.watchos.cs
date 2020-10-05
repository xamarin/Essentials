using System.Threading.Tasks;

namespace Xamarin.Essentials
{
    public static partial class Sms
    {
        internal static bool IsComposeSupported
            => ThrowHelper.ThrowNotImplementedException<bool>();

        static Task PlatformComposeAsync(SmsMessage message)
            => ThrowHelper.ThrowNotImplementedException<Task>();
    }
}
