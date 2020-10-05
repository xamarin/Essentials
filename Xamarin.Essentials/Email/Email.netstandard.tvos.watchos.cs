using System.Threading.Tasks;

namespace Xamarin.Essentials
{
    public static partial class Email
    {
        internal static bool IsComposeSupported =>
            ThrowHelper.ThrowNotImplementedException<bool>();

        static Task PlatformComposeAsync(EmailMessage message) =>
            ThrowHelper.ThrowNotImplementedException<Task>();
    }

#if NETSTANDARD1_0 || NETSTANDARD2_0
    public partial class EmailAttachment
    {
        string PlatformGetContentType(string extension) =>
            ThrowHelper.ThrowNotImplementedException<string>();
    }
#endif
}
