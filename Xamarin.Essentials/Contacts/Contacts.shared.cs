using System.Collections.Generic;
using System.Threading;
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

        public static Task<IEnumerable<Contact>> GetAllAsync(CancellationToken cancellationToken = default)
            => PlatformGetAllAsync(cancellationToken);

#if __IOS__ || __MACOS__ || TIZEN
        static string GetName(string name)
            => string.IsNullOrWhiteSpace(name) ? string.Empty : $" {name}";
#endif
    }

    public enum ContactPhoneType
    {
        Unknown = 0,
        Personal = 1,
        Work = 2,
        Main = 3,
        Mobile = 4
    }

    public enum ContactEmailType
    {
        Unknown = 0,
        Personal = 1,
        Work = 2
    }
}
