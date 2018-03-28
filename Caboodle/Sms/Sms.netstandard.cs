using System.Threading.Tasks;

namespace Microsoft.Caboodle
{
    public static partial class Sms
    {
        internal static bool IsComposeSupported => false;

        public static Task PlatformComposeAsync(SmsMessage message)
            => throw new NotImplementedInReferenceAssemblyException();
    }
}
