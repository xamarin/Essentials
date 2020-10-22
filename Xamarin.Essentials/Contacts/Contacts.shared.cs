using System.Collections.Generic;
using System.Threading.Tasks;

namespace Xamarin.Essentials
{
    public static partial class Contacts
    {
        public static async Task<Contact> PickContactAsync()
        {
            // iOS does not require permissions for the picker
            if (DeviceInfo.Platform != DevicePlatform.iOS)
                await Permissions.EnsureGrantedAsync<Permissions.ContactsRead>();
            return await PlatformPickContactAsync();
        }

#if !NETSTANDARD1_0
        public static IAsyncEnumerable<Contact> GetAllAsync()
            => PlatformGetAllAsync();
#endif
    }

    public enum ContactType
    {
        Unknown = 0,
        Personal = 1,
        Work = 2
    }
}
