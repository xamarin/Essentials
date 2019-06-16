using System.Threading.Tasks;

namespace Xamarin.Essentials
{
    public static partial class Contact
    {
        public static async Task<PhoneContact> PickContactAsync()
        {
            // await Permissions.RequireAsync(PermissionType.Contacts);
            return await PlataformPickContactAsync();
        }

        public static async Task SaveContactAsync(string name = null, string phone = null, string email = null)
        {
            // await Permissions.RequireAsync(PermissionType.Contacts);
            await PlataformSaveContactAsync(name, phone, email);
        }

        public static async Task SaveContact(PhoneContact contact)
        {
            // await Permissions.RequireAsync(PermissionType.Contacts);
            await PlataformSaveContact(contact);
        }
    }

    public enum ContactType
    {
        Unknow = 0,
        Personal = 1,
        Work = 2
    }
}
