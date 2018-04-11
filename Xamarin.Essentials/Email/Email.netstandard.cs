using System.Threading.Tasks;

namespace Xamarin.Essentials
{
    public static partial class Email
    {
        internal static bool IsComposeSupported
            => throw new NotImplementedInReferenceAssemblyException();

        private static Task PlatformComposeAsync(EmailMessage message)
            => throw new NotImplementedInReferenceAssemblyException();
    }
}
