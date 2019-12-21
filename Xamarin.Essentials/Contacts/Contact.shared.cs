using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.Essentials
{
    public static partial class Contact
    {
        public static async Task<PhoneContact> PickContactAsync()
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
                await Permissions.RequestAsync<Permissions.ContactsRead>().ConfigureAwait(false);
                await Permissions.RequestAsync<Permissions.ContactsWrite>().ConfigureAwait(false);
            }
        }
    }

    public enum ContactType
    {
        Unknow = 0,
        Personal = 1,
        Work = 2
    }
}
