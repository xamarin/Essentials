using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.Essentials
{
    public static partial class Contact
    {
        // TODO: Add permission

        public static Task<PhoneContact> PickContactAsync() => PlataformPickContactAsync();

        public static Task SaveContactAsync(string name = null, string phone = null, string email = null) => PlataformSaveContactAsync(name, phone, email);
    }
}
