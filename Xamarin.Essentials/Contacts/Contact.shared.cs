using System.Threading.Tasks;

namespace Xamarin.Essentials
{
    public static partial class Contact
    {
        public static async Task<PhoneContact> PickContactAsync()
        {
            await CheckPermissionAsync();
            return await PlataformPickContactAsync();
        }

        public static async Task SaveContactAsync(string name = null, string phone = null, string email = null)
        {
            await CheckPermissionAsync();
            await PlataformSaveContactAsync(name, phone, email);
        }

        public static async Task SaveContact(PhoneContact contact)
        {
            await CheckPermissionAsync();
            await PlataformSaveContact(contact);
        }

        static async Task CheckPermissionAsync()
        {
            if (DeviceInfo.Platform != DevicePlatform.iOS)
                await Permissions.RequireAsync(PermissionType.Contacts);
        }
    }

    public enum ContactType
    {
        Unknow = 0,
        Personal = 1,
        Work = 2
    }
}
