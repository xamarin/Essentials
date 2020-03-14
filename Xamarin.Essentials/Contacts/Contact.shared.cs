using System.Threading.Tasks;

namespace Xamarin.Essentials
{
    public static partial class Contact
    {
        public static async Task<PhoneContact?> PickContactAsync()
        {
            await CheckPermissionAsync();
            return await PlatformPickContactAsync();
        }

        public static async Task SaveContactAsync(string name = null, string phone = null, string email = null)
        {
            await CheckPermissionAsync();
            await PlatformSaveContactAsync(name, phone, email);
        }

        static async Task CheckPermissionAsync()
        {
            if (DeviceInfo.Platform != DevicePlatform.iOS)
            {
                await Permissions.RequestAsync<Permissions.ContactsRead>();
                await Permissions.RequestAsync<Permissions.ContactsWrite>();
            }
        }
    }

    public enum ContactType
    {
        Unknown = 0,
        Personal = 1,
        Work = 2
    }
}
