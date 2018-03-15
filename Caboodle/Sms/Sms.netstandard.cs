using System.Threading.Tasks;

namespace Microsoft.Caboodle
{
    public static partial class Sms
    {
        public static bool IsComposeSupported
            => throw new NotImplementedInReferenceAssemblyException();

        public static Task ComposeAsync(SmsMessage message)
            => throw new NotImplementedInReferenceAssemblyException();
    }
}
