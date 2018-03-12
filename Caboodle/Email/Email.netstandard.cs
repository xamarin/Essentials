using System.Threading.Tasks;

namespace Microsoft.Caboodle
{
    public static partial class Email
    {
        public static async void Compose(
            string[] recipientsto,
            string[] recipientscc,
            string[] recipientsbcc,
            string subject,
            string body,
            string bodymimetype,
            string[] attachmentspaths) =>
                throw new NotImplentedInReferenceAssembly();
    }
}
