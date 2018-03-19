using System.Threading.Tasks;

namespace Microsoft.Caboodle
{
    public static partial class Email
    {
        /// Indicating whether Compose Dialog is available
        public static bool IsComposeSupported
            => throw new NotImplementedInReferenceAssemblyException();

        /// Indicating whether Sending in background is available
        public static bool IsSendSupported
            => throw new NotImplementedInReferenceAssemblyException();

        public static async Task ComposeAsync(EmailMessage message) =>
                throw new NotImplementedInReferenceAssemblyException();

        public static async Task ComposeAsync(
            string[] recipientsto,
            string[] recipientscc,
            string[] recipientsbcc,
            string subject,
            string body,
            string bodymimetype,
            string[] attachmentspaths) =>
                throw new NotImplementedInReferenceAssemblyException();

        public static async void SendAsync(EmailMessage message) =>
                throw new NotImplementedInReferenceAssemblyException();
    }
}
